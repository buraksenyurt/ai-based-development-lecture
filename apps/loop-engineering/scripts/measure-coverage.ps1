<#
.SYNOPSIS
    Döngünün ölçütü: Orders modülünün satır kapsamını (line coverage) ölçer.

.DESCRIPTION
    Testleri coverlet ile çalıştırır, OpenCover raporlarını birleştirir ve yolu
    "Orders" klasörü içeren dosyalar (src/LoopLab.Domain/Orders ve
    src/LoopLab.Application/Orders) için satır kapsamını hesaplar.
    Aynı satır birden fazla test projesinin raporunda geçerse en yüksek ziyaret
    sayısı alınır, yani satır bir kez sayılır.

    Son satırda makinenin okuyabileceği tek bir değer yazar:
        ORDERS_LINE_COVERAGE=35.7

    SonarQube "Coverage" değeri satır + dal kapsamını birlikte hesapladığı için
    bu değerle doğrudan karşılaştırılmamalıdır. Sonar insan kapısıdır, döngünün
    ölçütü bu betiktir.

.PARAMETER Target
    Hedef kapsam yüzdesi (varsayılan 70).

.PARAMETER Check
    Verilirse kapsam hedefin altındayken exit code 1 döner (hook/CI için).

.EXAMPLE
    ./scripts/measure-coverage.ps1

.EXAMPLE
    ./scripts/measure-coverage.ps1 -Check
#>
[CmdletBinding()]
param(
    [double]$Target = 70,
    [switch]$Check
)

$ErrorActionPreference = "Stop"

$solutionRoot = Split-Path -Parent $PSScriptRoot
$resultsDir   = Join-Path $solutionRoot "TestResults/measure"

Push-Location $solutionRoot
try {
    if (Test-Path $resultsDir) {
        Remove-Item -Recurse -Force $resultsDir
    }

    Write-Host "==> dotnet test (coverage)" -ForegroundColor Cyan
    dotnet test LoopLab.slnx `
        --collect:"XPlat Code Coverage" `
        --results-directory "$resultsDir" `
        --verbosity quiet `
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover | Out-Host

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Testler başarısız oldu (exit code: $LASTEXITCODE). Kapsam ölçülmedi."
        exit 2
    }

    $reports = Get-ChildItem -Path $resultsDir -Recurse -Filter "coverage.opencover.xml"
    if (-not $reports) {
        Write-Error "coverage.opencover.xml bulunamadı."
        exit 2
    }

    # dosya yolu -> (satır -> en yüksek ziyaret sayısı)
    $coverage = @{}

    foreach ($report in $reports) {
        [xml]$xml = Get-Content -Path $report.FullName -Raw -Encoding UTF8

        foreach ($module in $xml.CoverageSession.Modules.Module) {
            $files = @{}
            foreach ($file in $module.Files.File) {
                $files[$file.uid] = ($file.fullPath -replace '\\', '/')
            }

            foreach ($class in $module.Classes.Class) {
                foreach ($method in $class.Methods.Method) {
                    foreach ($point in $method.SequencePoints.SequencePoint) {
                        $path = $files[$point.fileid]
                        if (-not $path -or $path -notmatch '/Orders/') { continue }

                        if (-not $coverage.ContainsKey($path)) {
                            $coverage[$path] = @{}
                        }

                        $line   = [int]$point.sl
                        $visits = [int]$point.vc
                        if (-not $coverage[$path].ContainsKey($line) -or $coverage[$path][$line] -lt $visits) {
                            $coverage[$path][$line] = $visits
                        }
                    }
                }
            }
        }
    }

    if ($coverage.Count -eq 0) {
        Write-Error "Raporlarda Orders modülüne ait dosya bulunamadı."
        exit 2
    }

    $totalLines   = 0
    $coveredLines = 0
    $rows = foreach ($path in ($coverage.Keys | Sort-Object)) {
        $lines   = $coverage[$path]
        $total   = $lines.Count
        $covered = @($lines.Values | Where-Object { $_ -gt 0 }).Count
        $totalLines   += $total
        $coveredLines += $covered

        [pscustomobject]@{
            Kapsam = "{0,6:N1}%" -f (100.0 * $covered / $total)
            Satir  = "$covered/$total"
            Dosya  = ($path -replace '^.*?/src/', 'src/')
        }
    }

    $rows | Sort-Object Kapsam | Format-Table -AutoSize | Out-Host

    $percent = [math]::Round(100.0 * $coveredLines / $totalLines, 1)
    $invariant = [System.Globalization.CultureInfo]::InvariantCulture

    Write-Host ("Orders modülü: {0}/{1} satır, hedef %{2}" -f $coveredLines, $totalLines, $Target)
    Write-Output ("ORDERS_LINE_COVERAGE={0}" -f $percent.ToString("0.0", $invariant))

    if ($Check -and $percent -lt $Target) {
        exit 1
    }
    exit 0
}
finally {
    Pop-Location
}
