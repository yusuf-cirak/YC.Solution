#!/usr/bin/env pwsh

<#
.SYNOPSIS
Local version synchronization and management utility for YC.Solution packages.

.DESCRIPTION
Provides local version checking, updating, and validation for NuGet packages.
Reduces need for git commits during development.

.PARAMETER Action
The action to perform: Check, Update, or Validate

.PARAMETER Package
Package name: YC.Monad or YC.Monad.EntityFrameworkCore

.PARAMETER Version
New version number (required for Update action)

.EXAMPLE
.\version-sync.ps1 -Action Check
.\version-sync.ps1 -Action Update -Package "YC.Monad" -Version "1.4.0"
.\version-sync.ps1 -Action Validate
#>

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('Check', 'Update', 'Validate')]
    [string]$Action,

    [Parameter(Mandatory=$false)]
    [ValidateSet('YC.Monad', 'YC.Monad.EntityFrameworkCore', 'Both')]
    [string]$Package = 'Both',

    [Parameter(Mandatory=$false)]
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$config = Get-Content '.github/version-config.json' | ConvertFrom-Json

function Get-PackageVersion {
    param([string]$CsprojPath)

    if (-not (Test-Path $CsprojPath)) {
        throw "Project file not found: $CsprojPath"
    }

    $content = Get-Content $CsprojPath -Raw
    if ($content -match '<PackageVersion>(.*?)</PackageVersion>') {
        return $matches[1]
    }

    throw "PackageVersion not found in $CsprojPath"
}

function Set-PackageVersion {
    param(
        [string]$CsprojPath,
        [string]$NewVersion
    )

    if (-not (Test-Path $CsprojPath)) {
        throw "Project file not found: $CsprojPath"
    }

    $content = Get-Content $CsprojPath -Raw
    $content = $content -replace '<PackageVersion>.*?</PackageVersion>', "<PackageVersion>$NewVersion</PackageVersion>"
    Set-Content $CsprojPath $content -Encoding UTF8
    Write-Host "Updated $(Split-Path $CsprojPath -Leaf): $NewVersion" -ForegroundColor Green
}

function Test-SemanticVersion {
    param([string]$Version)

    if ($Version -match '^[0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9.]+)?(\+[a-zA-Z0-9.]+)?$') {
        return $true
    }
    return $false
}

function Invoke-CheckAction {
    Write-Host ""
    Write-Host "=== Package Versions ===" -ForegroundColor Cyan

    foreach ($pkg in $config.packages) {
        $version = Get-PackageVersion $pkg.csprojPath
        Write-Host "$($pkg.name): $version" -ForegroundColor Yellow
    }
}

function Invoke-UpdateAction {
    if (-not $Version) {
        throw "Version parameter is required for Update action"
    }

    if (-not (Test-SemanticVersion $Version)) {
        throw "Invalid semantic version format: $Version. Expected: X.Y.Z"
    }

    if ($Package -eq 'Both') {
        $packagesToUpdate = $config.packages
    } else {
        $packagesToUpdate = @($config.packages | Where-Object { $_.name -eq $Package })
    }

    Write-Host ""
    Write-Host "=== Updating Versions ===" -ForegroundColor Cyan

    foreach ($pkg in $packagesToUpdate) {
        $oldVersion = Get-PackageVersion $pkg.csprojPath
        Set-PackageVersion $pkg.csprojPath $Version
        Write-Host "  From: $oldVersion -> To: $Version" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "All versions updated successfully" -ForegroundColor Green
}

function Invoke-ValidateAction {
    Write-Host ""
    Write-Host "=== Validating Versions ===" -ForegroundColor Cyan

    $allValid = $true

    foreach ($pkg in $config.packages) {
        $version = Get-PackageVersion $pkg.csprojPath

        if (Test-SemanticVersion $version) {
            Write-Host "OK - $($pkg.name): $version" -ForegroundColor Green
        } else {
            Write-Host "ERROR - $($pkg.name): $version - Invalid format!" -ForegroundColor Red
            $allValid = $false
        }
    }

    if ($allValid) {
        Write-Host ""
        Write-Host "All versions valid" -ForegroundColor Green
    } else {
        throw "Some versions are invalid"
    }
}

switch ($Action) {
    'Check' { Invoke-CheckAction }
    'Update' { Invoke-UpdateAction }
    'Validate' { Invoke-ValidateAction }
}
