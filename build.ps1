param(
    [string]$TargetPath
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

if ([string]::IsNullOrWhiteSpace($TargetPath)) {
    $TargetPath = Join-Path $root 'dist'
}
elseif (-not [System.IO.Path]::IsPathRooted($TargetPath)) {
    $TargetPath = Join-Path $root $TargetPath
}

$output = Join-Path $TargetPath 'CalScreenerConverter'

dotnet publish (Join-Path $root 'Converter\Converter.csproj') `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    --output $output

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

New-Item -ItemType Directory -Force -Path (Join-Path $output 'Input') | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $output 'Output') | Out-Null
Copy-Item (Join-Path $root 'README.md') $output

Write-Host "Build complete: $output"