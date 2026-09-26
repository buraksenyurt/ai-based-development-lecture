<#
.SYNOPSIS
    ProjectRouter solution sends to SonarQube with test coverage.

.DESCRIPTION
    Steps: begin -> build -> test (OpenCover coverage + TRX) -> end
    Token, via the -Token parameter or the SONAR_TOKEN environment variable.

.EXAMPLE
    $env:SONAR_TOKEN = "sqp_..."
    .\sonar-scan.ps1

.EXAMPLE
    .\sonar-scan.ps1 -Token "sqp_..." -HostUrl "http://localhost:9000"
#>
[CmdletBinding()]
param(
    [string]$ProjectKey = "project-router",
    [string]$HostUrl    = "http://localhost:9000",
    [string]$Token      = $env:SONAR_TOKEN
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($Token)) {
    Write-Error "SonarQube token can't find. -Give a Token via the -Token parameter or set the `$env:SONAR_TOKEN environment variable before running this script."
    exit 1
}

# Stop the script if a command fails
function Invoke-Step {
    param([string]$Title, [scriptblock]$Command)
    Write-Host ""
    Write-Host "==> $Title" -ForegroundColor Cyan
    & $Command
    if ($LASTEXITCODE -ne 0) {
        Write-Error "'$Title' step failed (exit code: $LASTEXITCODE)."
        exit $LASTEXITCODE
    }
}

Push-Location $PSScriptRoot
try {
    $resultsDir = Join-Path $PSScriptRoot "TestResults"

    # Remove reports from previous runs to avoid confusion
    if (Test-Path $resultsDir) {
        Remove-Item -Recurse -Force $resultsDir
    }

    Invoke-Step "SonarScanner begin" {
        dotnet sonarscanner begin `
            /k:"$ProjectKey" `
            /d:sonar.host.url="$HostUrl" `
            /d:sonar.token="$Token" `
            /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml" `
            /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx"
    }

    Invoke-Step "Build" {
        dotnet build --no-incremental
    }

    Invoke-Step "Test + coverage (OpenCover)" {
        dotnet test --no-build `
            --collect:"XPlat Code Coverage" `
            --results-directory "$resultsDir" `
            --logger trx `
            -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
    }

    $reports = Get-ChildItem -Path $resultsDir -Recurse -Filter "coverage.opencover.xml" -ErrorAction SilentlyContinue
    if (-not $reports) {
        Write-Warning "coverage.opencover.xml not found; SonarQube will show 0% coverage."
    } else {
        Write-Host "Found coverage reports:" -ForegroundColor Green
        $reports | ForEach-Object { Write-Host "  $($_.FullName)" }
    }

    Invoke-Step "SonarScanner end" {
        dotnet sonarscanner end /d:sonar.token="$Token"
    }

    Write-Host ""
    Write-Host "Completed: $HostUrl/dashboard?id=$ProjectKey" -ForegroundColor Green
}
finally {
    Pop-Location
}
