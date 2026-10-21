<#
.SYNOPSIS
    LoopLab solution'ını test kapsamı ile birlikte SonarQube'a gönderir.

.DESCRIPTION
    Adımlar: begin -> build -> test (OpenCover coverage + TRX) -> end
    Token, -Token parametresi ya da SONAR_TOKEN_LOOP_LAB ortam değişkeni ile verilir.

.EXAMPLE
    $env:SONAR_TOKEN_LOOP_LAB = "sqp_..."
    ./scripts/sonar-scan.ps1

.EXAMPLE
    ./scripts/sonar-scan.ps1 -Token "sqp_..." -HostUrl "http://localhost:9000"
#>
[CmdletBinding()]
param(
    [string]$ProjectKey = "loop-lab",
    [string]$HostUrl    = "http://localhost:9000",
    [string]$Token      = $env:SONAR_TOKEN_LOOP_LAB
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($Token)) {
    Write-Error "SonarQube token bulunamadı. -Token parametresini kullanın ya da çalıştırmadan önce `$env:SONAR_TOKEN_LOOP_LAB ortam değişkenini tanımlayın."
    exit 1
}

# Bir adım başarısız olursa betiği durdurur
function Invoke-Step {
    param([string]$Title, [scriptblock]$Command)
    Write-Host ""
    Write-Host "==> $Title" -ForegroundColor Cyan
    & $Command
    if ($LASTEXITCODE -ne 0) {
        Write-Error "'$Title' adımı başarısız oldu (exit code: $LASTEXITCODE)."
        exit $LASTEXITCODE
    }
}

# Betik scripts/ altında; solution kök klasörde
$solutionRoot = Split-Path -Parent $PSScriptRoot

Push-Location $solutionRoot
try {
    $resultsDir = Join-Path $solutionRoot "TestResults"

    # Önceki çalıştırmalardan kalan raporları temizle
    if (Test-Path $resultsDir) {
        Remove-Item -Recurse -Force $resultsDir
    }

    Invoke-Step "SonarScanner begin" {
        dotnet sonarscanner begin `
            /k:"$ProjectKey" `
            /d:sonar.host.url="$HostUrl" `
            /d:sonar.token="$Token" `
            /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml" `
            /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx" `
            /d:sonar.coverage.exclusions="**/Program.cs"
    }

    Invoke-Step "Build" {
        dotnet build LoopLab.slnx --no-incremental
    }

    Invoke-Step "Test + coverage (OpenCover)" {
        dotnet test LoopLab.slnx --no-build `
            --collect:"XPlat Code Coverage" `
            --results-directory "$resultsDir" `
            --logger trx `
            -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
    }

    $reports = Get-ChildItem -Path $resultsDir -Recurse -Filter "coverage.opencover.xml" -ErrorAction SilentlyContinue
    if (-not $reports) {
        Write-Warning "coverage.opencover.xml bulunamadı; SonarQube kapsamı %0 gösterecek."
    } else {
        Write-Host "Bulunan kapsam raporları:" -ForegroundColor Green
        $reports | ForEach-Object { Write-Host "  $($_.FullName)" }
    }

    Invoke-Step "SonarScanner end" {
        dotnet sonarscanner end /d:sonar.token="$Token"
    }

    Write-Host ""
    Write-Host "Tamamlandı: $HostUrl/dashboard?id=$ProjectKey" -ForegroundColor Green
}
finally {
    Pop-Location
}
