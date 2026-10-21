<#
.SYNOPSIS
    Döngünün tek bir turunu insan olmadan (headless) çalıştırır ve sonucu commit eder.

.DESCRIPTION
    Tetikleyicilerin (elle, Windows Görev Zamanlayıcı, CI) çağıracağı tek giriş noktasıdır.

    Adımlar:
      1. Ön kontroller: loop/ ile başlayan bir branch'te miyiz, çalışma alanı temiz mi?
      2. Durma koşulları: son kayıt "insan kararı gerekli" diyorsa, tur sınırına gelindiyse
         ya da hedef zaten sağlanmışsa turu hiç başlatmaz.
      3. claude -p ile turu çalıştırır. Stop hook'u (check-loop-goal.ps1) turun sonunda
         kuralları denetler.
      4. Değişiklikleri commit eder. Push ETMEZ; push ve merge insan kapısıdır.

    Çıkış kodları:
      0 = tur tamamlandı ya da durma koşulu sağlandığı için tur başlatılmadı
      1 = ön kontrol başarısız
      3 = claude çalıştı ama değişiklik üretmedi

.PARAMETER MaxTurns
    claude -p için en fazla ajan adımı (maliyet freni). Varsayılan 40.

.PARAMETER MaxLoopTurns
    LOOP.md'deki kaçış kapısı: en fazla tur sayısı. Varsayılan 8.

.PARAMETER NoCommit
    Turu çalıştırır ama commit etmez (gözlem için).

.EXAMPLE
    ./scripts/run-loop-turn.ps1

.EXAMPLE
    ./scripts/run-loop-turn.ps1 -MaxTurns 25 -NoCommit
#>
[CmdletBinding()]
param(
    [int]$MaxTurns = 40,
    [int]$MaxLoopTurns = 8,
    [switch]$NoCommit
)

$ErrorActionPreference = "Continue"

$root         = Split-Path -Parent $PSScriptRoot
$progressPath = Join-Path $root "PROGRESS.md"
$prompt       = "LOOP.md ve PROGRESS.md dosyalarını oku ve döngünün bir sonraki turunu çalıştır. " +
                "Turu bitirmeden önce test-reviewer alt ajanından onay al ve sonucu PROGRESS.md'ye kaydet. " +
                "Tek tur yap ve dur."

function Write-Step([string]$Text) { Write-Host "`n==> $Text" -ForegroundColor Cyan }

Push-Location $root
try {
    # --- 1) Ön kontroller ---------------------------------------------------
    Write-Step "Ön kontroller"

    if (-not (Get-Command claude -ErrorAction SilentlyContinue)) {
        Write-Host "claude komutu bulunamadı. Claude Code CLI kurulu ve PATH'te olmalı." -ForegroundColor Red
        exit 1
    }

    $branch = (git branch --show-current).Trim()
    if ($branch -notlike "loop/*") {
        Write-Host "Döngü yalnızca 'loop/' ile başlayan bir branch'te çalışır (şu an: $branch)." -ForegroundColor Red
        exit 1
    }

    $dirty = @(git status --porcelain -- .)
    if ($dirty.Count -gt 0) {
        Write-Host "apps/loop-engineering altında commit edilmemiş değişiklik var. Önce temizleyin:" -ForegroundColor Red
        $dirty | ForEach-Object { Write-Host "  $_" }
        exit 1
    }

    # --- 2) Durma koşulları -------------------------------------------------
    Write-Step "Durma koşulları"

    $progress = [System.IO.File]::ReadAllText($progressPath, [System.Text.Encoding]::UTF8)
    $turHits  = [regex]::Matches($progress, '(?m)^## Tur (\d+)')
    $lastTur  = if ($turHits.Count -gt 0) { [int]$turHits[$turHits.Count - 1].Groups[1].Value } else { 0 }
    $lastText = if ($turHits.Count -gt 0) { $progress.Substring($turHits[$turHits.Count - 1].Index) } else { "" }

    if ($lastText -match 'insan kararı gerekli') {
        Write-Host "Son kayıt (Tur $lastTur) 'insan kararı gerekli' diyor. Tur başlatılmadı." -ForegroundColor Yellow
        exit 0
    }

    if ($lastTur -ge $MaxLoopTurns) {
        Write-Host "Tur sınırına ($MaxLoopTurns) ulaşıldı. Tur başlatılmadı; PROGRESS.md'yi inceleyin." -ForegroundColor Yellow
        exit 0
    }

    $psExe   = (Get-Process -Id $PID).Path
    $measure = Join-Path $PSScriptRoot "measure-coverage.ps1"
    & $psExe -NoProfile -ExecutionPolicy Bypass -File $measure -Check | Out-Host
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Hedef zaten sağlanmış. Tur başlatılmadı." -ForegroundColor Green
        exit 0
    }
    if ($LASTEXITCODE -ne 1) {
        Write-Host "Ölçüm başarısız (exit code: $LASTEXITCODE). Testleri kontrol edin." -ForegroundColor Red
        exit 1
    }

    # --- 3) Tur ---------------------------------------------------------------
    $nextTur = $lastTur + 1
    Write-Step "Tur $nextTur çalıştırılıyor (en fazla $MaxTurns adım)"

    # acceptEdits: dosya düzenlemeleri onay beklemez; settings.json'daki deny kuralları
    # (src/** yasağı) yine geçerlidir.
    claude -p $prompt --max-turns $MaxTurns --permission-mode acceptEdits | Out-Host
    $claudeExit = $LASTEXITCODE

    # --- 4) Sonuç ve commit -------------------------------------------------
    Write-Step "Sonuç"

    $changes = @(git status --porcelain -- .)
    if ($changes.Count -eq 0) {
        Write-Host "claude değişiklik üretmedi (exit code: $claudeExit)." -ForegroundColor Yellow
        exit 3
    }
    git status --short -- . | Out-Host

    $progress = [System.IO.File]::ReadAllText($progressPath, [System.Text.Encoding]::UTF8)
    $block    = [regex]::Match($progress, "(?ms)^## Tur $nextTur\b.*?(?=^## Tur |\z)").Value
    $sinif    = ([regex]::Match($block, '(?m)^Sınıf:\s*([^\s(]+)')).Groups[1].Value
    $kapsam   = ([regex]::Match($block, '%\s*(\d+(?:[.,]\d+)?)\s*(?:\u2192|->)\s*%\s*(\d+(?:[.,]\d+)?)'))

    $summary = if ($sinif) { $sinif } else { "no progress entry" }
    if ($kapsam.Success) { $summary += " ($($kapsam.Groups[1].Value)% -> $($kapsam.Groups[2].Value)%)" }
    if ($block -match 'insan kararı gerekli') { $summary += " - human decision needed" }

    if ($NoCommit) {
        Write-Host "NoCommit verildi; değişiklikler commit edilmedi." -ForegroundColor Yellow
        exit 0
    }

    git add -- tests PROGRESS.md
    git commit -m "test(loop-lab): tur $nextTur - $summary" `
               -m "Automated loop turn (scripts/run-loop-turn.ps1). Review the diff before merging." `
               -m "Co-Authored-By: Claude <noreply@anthropic.com>" | Out-Host

    $left = @(git status --porcelain -- .)
    if ($left.Count -gt 0) {
        Write-Host "Commit dışında kalan değişiklikler var (tests/ ve PROGRESS.md dışı):" -ForegroundColor Yellow
        $left | ForEach-Object { Write-Host "  $_" }
    }

    Write-Host "`nTur $nextTur tamamlandı. Push ve PR insan kapısıdır." -ForegroundColor Green
    exit 0
}
finally {
    Pop-Location
}
