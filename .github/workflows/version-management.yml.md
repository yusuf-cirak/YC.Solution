# Version Management Workflow

## Overview

This workflow provides manual version management capabilities through the GitHub UI. It serves as an alternative to local scripts for checking, updating, and validating package versions.

## Triggers

- **Manual**: Triggered manually from GitHub Actions UI only

## What It Does

Based on the selected action:

### Check Action
- Lists all packages and their current versions
- Reads from `.csproj` files
- Displays version information

### Update Action
- Updates `PackageVersion` in selected `.csproj` files
- Validates semantic version format
- Commits and pushes changes automatically

### Validate Action
- Checks all versions follow semantic versioning (X.Y.Z)
- Reports invalid versions
- Ensures consistency across packages

## Required Secrets

None - uses GitHub's built-in token (`GITHUB_TOKEN`) for commits

## Manual Inputs

- `action`: Action to perform (`check`, `update`, `validate`)
- `package`: Package to update (required for `update` action)
- `new_version`: New version number (required for `update` action)

## Actions Explained

### Check
Lists current versions of all packages:
```
YC.Monad: 1.3.0
YC.Monad.EntityFrameworkCore: 1.2.0
```

### Update
Updates specified package to new version:
- Validates version format
- Updates `.csproj` file
- Commits with message: `chore: update package versions to X.Y.Z`

### Validate
Checks version format compliance:
- Must follow X.Y.Z pattern
- Can include pre-release identifiers (-alpha, -beta)
- Can include build metadata (+build)

## Troubleshooting

### Update Fails
- Check version format (must be semantic: X.Y.Z)
- Ensure package name matches configuration
- Verify repository write permissions

### Validation Fails
- Check version strings in `.csproj` files
- Ensure no invalid characters
- Verify semantic versioning compliance

### Commit Fails
- Check repository permissions
- Ensure branch is not protected from direct pushes
- Verify no merge conflicts

## Manual Trigger

To manage versions:
1. Go to **Actions** tab
2. Select **"Version Management"**
3. Click **"Run workflow"**
4. Configure inputs based on action:

### For Checking Versions
- **Action**: `check`
- **Package**: (leave empty)
- **New version**: (leave empty)

### For Updating Versions
- **Action**: `update`
- **Package**: Select `YC.Monad`, `YC.Monad.EntityFrameworkCore`, or `Both`
- **New version**: Enter version like `1.4.0`

### For Validating Versions
- **Action**: `validate`
- **Package**: (leave empty)
- **New version**: (leave empty)

## Example Usage

**Check current versions:**
- Action: `check`
- Shows all package versions

**Update YC.Monad to 1.4.0:**
- Action: `update`
- Package: `YC.Monad`
- New version: `1.4.0`

**Validate all versions:**
- Action: `validate`
- Reports any invalid versions

## Version Format Requirements

Valid formats:
- `1.0.0` - Release version
- `1.0.0-alpha` - Pre-release
- `1.0.0+build.1` - With build metadata
- `2.0.0-beta.1+build.123` - Full format

Invalid formats:
- `1.0` - Missing patch version
- `1.0.0.0` - Too many parts
- `v1.0.0` - Leading 'v'

## Related Files

- `.github/version-config.json` - Package configuration
- `src/**/*.csproj` - Project files with version info
- `scripts/version-sync.ps1` - Local alternative (if available)</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\version-management.yml.md
