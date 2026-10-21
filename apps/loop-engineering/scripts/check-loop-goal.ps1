<#
.SYNOPSIS
    Stop hook: ajan durmaya çalıştığında turun LOOP.md kurallarına uyup uymadığını denetler.

.DESCRIPTION
    Claude Code bu betiği .claude/settings.json içindeki "Stop" hook'u olarak çalıştırır.
    Hook girdisi (JSON) stdin'den okunur.

    Davranış:
      - apps/loop-engineering altında değişiklik yoksa (tur yapılmamışsa) durmaya izin verir.
      - Son tur kaydı "insan kararı gerekli" diyorsa durmaya izin verir.
      - Aksi halde şunları kontrol eder:
          1. src/ altında değişiklik var mı?
          2. tests/ altındaki değişiklik 300 satırı aşıyor mu?
          3. PROGRESS.md'ye yeni bir "## Tur N" kaydı eklenmiş mi?
          4. Son kayıtta "Reviewer: ONAY" var mı?
          5. Testler geçiyor mu ve measure-coverage.ps1 sonucu PROGRESS.md'ye yazılan
             "sonra" değeriyle aynı mı?
      - Bir kural ihlal edilmişse exit code 2 ile çıkar; stderr'deki gerekçe ajana
        geri bildirim olarak gider ve ajan çalışmaya devam eder.
      - Ajan zaten bir kez geri gönderilmişse (stop_hook_active) tekrar engellemez.
        Bunun yerine bulguları PROGRESS.md'ye "insan kararı gerekli" notu olarak yazar
        ve durmaya izin verir. Böylece hook sonsuz döngü yaratmaz.

    Geçici olarak devre dışı bırakmak için: $env:LOOP_HOOK_DISABLED = "1"
#>
# Windows PowerShell 5.1'de native komutların stderr çıktısı "Stop" modunda istisnaya
# dönüşebildiği için hatalar betik içinde açıkça ele alınır.
$ErrorActionPreference = "Continue"

if ($env:LOOP_HOOK_DISABLED -eq "1") { exit 0 }

# Türkçe gerekçelerin Claude Code'a bozulmadan ulaşması için
try { [Console]::OutputEncoding = [System.Text.Encoding]::UTF8 } catch { }

$utf8NoBom = New-Object System.Text.UTF8Encoding $false
$arrow     = [string][char]0x2192   # "→"

# --- Hook girdisi -----------------------------------------------------------
$hookActive = $false
try {
    $raw = [Console]::In.ReadToEnd()
    if (-not [string]::IsNullOrWhiteSpace($raw)) {
        $payload = $raw | ConvertFrom-Json
        if ($payload.stop_hook_active -eq $true) { $hookActive = $true }
        $sessionId = "$($payload.session_id)"
    }
} catch {
    # Girdi okunamazsa ilk deneme gibi davran
}

$root         = Split-Path -Parent $PSScriptRoot

# stop_hook_active alanına ek olarak kendi korumamız: aynı oturumda bir kez engellediysek
# tekrar engellemeyiz. (TestResults klasörü .gitignore kapsamında.)
if ([string]::IsNullOrWhiteSpace($sessionId)) { $sessionId = "no-session" }
$guardDir  = Join-Path $root "TestResults"
$guardFile = Join-Path $guardDir ".stop-hook-$sessionId"
if (Test-Path $guardFile) { $hookActive = $true }
$progressPath = Join-Path $root "PROGRESS.md"
$maxDiffLines = 300
$tolerance    = 0.1

function Invoke-Git {
    param([string[]]$Arguments)
    $output = & git -C $root @Arguments 2>$null
    if ($null -eq $output) { return @() }
    return @($output)
}

function Get-TurBlocks {
    param([string]$Text)
    $blocks = @()
    $hits   = [regex]::Matches($Text, '(?m)^## Tur (\d+)')
    for ($i = 0; $i -lt $hits.Count; $i++) {
        $start = $hits[$i].Index
        $end   = if ($i + 1 -lt $hits.Count) { $hits[$i + 1].Index } else { $Text.Length }
        $blocks += [pscustomobject]@{
            Number = [int]$hits[$i].Groups[1].Value
            Text   = $Text.Substring($start, $end - $start)
        }
    }
    return $blocks
}

# --- 0) Tur yapılmış mı? ----------------------------------------------------
# Yalnızca döngünün dokunduğu yerlere bakılır; README gibi insan düzenlemeleri hook'u tetiklemez.
$changed = Invoke-Git @("status", "--porcelain", "--", "tests", "src", "PROGRESS.md")
if ($changed.Count -eq 0) { exit 0 }

$progressNow  = [System.IO.File]::ReadAllText($progressPath, [System.Text.Encoding]::UTF8)
$progressHead = (Invoke-Git @("show", "HEAD:./PROGRESS.md")) -join "`n"

$blocksNow  = Get-TurBlocks $progressNow
$blocksHead = Get-TurBlocks $progressHead
$lastNow    = $blocksNow | Select-Object -Last 1
$lastHead   = $blocksHead | Select-Object -Last 1

if ($lastNow -and $lastNow.Text -match 'insan kararı gerekli') {
    exit 0
}

$problems = New-Object System.Collections.Generic.List[string]

# --- 1) src/ altına dokunulmuş mu? -----------------------------------------
$srcChanges = @(Invoke-Git @("diff", "--relative", "--name-only", "HEAD", "--", "src")) +
              @(Invoke-Git @("ls-files", "--others", "--exclude-standard", "--", "src"))
if ($srcChanges.Count -gt 0) {
    $problems.Add("src/ altında değişiklik var, LOOP.md üretim koduna dokunmayı yasaklıyor: $($srcChanges -join ', '). Bu değişiklikleri geri al.")
}

# --- 2) Diff boyutu --------------------------------------------------------
$diffLines = 0
foreach ($line in (Invoke-Git @("diff", "--relative", "--numstat", "HEAD", "--", "tests"))) {
    $parts = $line -split "`t"
    if ($parts[0] -match '^\d+$') { $diffLines += [int]$parts[0] }
    if ($parts[1] -match '^\d+$') { $diffLines += [int]$parts[1] }
}
foreach ($file in (Invoke-Git @("ls-files", "--others", "--exclude-standard", "--", "tests"))) {
    $diffLines += @(Get-Content -Path (Join-Path $root $file)).Count
}
if ($diffLines -gt $maxDiffLines) {
    $problems.Add("tests/ altındaki değişiklik $diffLines satır; LOOP.md sınırı $maxDiffLines satır. Turu küçült.")
}

# --- 3) Yeni tur kaydı -----------------------------------------------------
$headNumber = if ($lastHead) { $lastHead.Number } else { -1 }
if (-not $lastNow -or $lastNow.Number -le $headNumber) {
    $problems.Add("PROGRESS.md'ye yeni bir '## Tur $($headNumber + 1)' kaydı eklenmemiş.")
}

# --- 4) Reviewer onayı -----------------------------------------------------
if ($lastNow -and $lastNow.Number -gt $headNumber -and $lastNow.Text -notmatch 'Reviewer:\s*ONAY') {
    $problems.Add("Son tur kaydında 'Reviewer: ONAY' yok. test-reviewer alt ajanından onay al ya da RED gerekçesini düzelt.")
}

# --- 5) Testler ve ölçüm ---------------------------------------------------
$psExe   = (Get-Process -Id $PID).Path
$measure = Join-Path $PSScriptRoot "measure-coverage.ps1"
$output  = & $psExe -NoProfile -ExecutionPolicy Bypass -File $measure 2>&1 | ForEach-Object { "$_" }
$measureExit = $LASTEXITCODE

$measured = $null
$valueLine = $output | Where-Object { $_ -match '^ORDERS_LINE_COVERAGE=' } | Select-Object -Last 1
if ($valueLine) {
    $measured = [double]::Parse(($valueLine -split '=')[1], [System.Globalization.CultureInfo]::InvariantCulture)
}

if ($measureExit -ne 0 -or $null -eq $measured) {
    $problems.Add("measure-coverage.ps1 başarısız oldu (exit code: $measureExit). Testlerden biri kırık olabilir; 'dotnet test LoopLab.slnx' ile kontrol et.")
}
elseif ($lastNow -and $lastNow.Number -gt $headNumber) {
    $kapsamLine = ($lastNow.Text -split "`n") | Where-Object { $_ -match '^Kapsam:' } | Select-Object -First 1
    $pattern    = '(?:' + [regex]::Escape($arrow) + '|->)\s*%?\s*(\d+(?:[.,]\d+)?)'
    $written    = $null
    if ($kapsamLine -and $kapsamLine -match $pattern) {
        $written = [double]::Parse($Matches[1].Replace(',', '.'), [System.Globalization.CultureInfo]::InvariantCulture)
    }

    if ($null -eq $written) {
        $problems.Add("Son tur kaydının 'Kapsam:' satırında '%X $arrow %Y' biçiminde bir değer bulunamadı. Ölçülen değer: %$measured.")
    }
    elseif ([math]::Abs($written - $measured) -gt $tolerance) {
        $problems.Add("PROGRESS.md'de yazan kapsam (%$written) ile measure-coverage.ps1 sonucu (%$measured) uyuşmuyor. Değeri tahmin etme; betiğin çıktısını yaz.")
    }
}

# --- Karar -----------------------------------------------------------------
if ($problems.Count -eq 0) {
    Remove-Item -Path $guardFile -ErrorAction SilentlyContinue
    exit 0
}

$message = "Tur LOOP.md kurallarını karşılamıyor:`n- " + ($problems -join "`n- ")

if ($hookActive) {
    # İkinci kez engelleme: durmaya izin ver, durumu kalıcı olarak kaydet.
    $note = "`r`nHook ($(Get-Date -Format 'yyyy-MM-dd HH:mm')): Ajan bir kez geri gönderildi ama şu sorunlar sürüyor: " +
            ($problems -join " | ") + " İnsan kararı gerekli.`r`n"
    [System.IO.File]::AppendAllText($progressPath, $note, $utf8NoBom)
    Remove-Item -Path $guardFile -ErrorAction SilentlyContinue
    exit 0
}

if (-not (Test-Path $guardDir)) { New-Item -ItemType Directory -Path $guardDir | Out-Null }
[System.IO.File]::WriteAllText($guardFile, (Get-Date -Format o), $utf8NoBom)

[Console]::Error.WriteLine($message)
exit 2
