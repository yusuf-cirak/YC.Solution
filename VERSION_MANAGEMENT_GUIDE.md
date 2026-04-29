# Version Management Guide

## Overview

This project uses an automated version management and release pipeline to:
- Detect version changes in package projects
- Generate changelogs following Keep a Changelog format
- Publish packages to NuGet
- Create GitHub releases and tags

## Conventional Commits

All commit messages should follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

### Format

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Changes that don't affect code meaning (formatting, etc)
- **refactor**: Code change that neither fixes a bug nor adds a feature
- **perf**: Code change that improves performance
- **test**: Adding or updating tests
- **chore**: Changes to build process or dependencies
- **ci**: Changes to CI configuration

### Breaking Changes

To indicate breaking changes, add a footer:

```
feat: send an email to the customer when a product is shipped

BREAKING CHANGE: The 'email' parameter is now required (was optional).
```

## Version Management Workflows

### 1. Automatic Version Detection (`detect-version-changes.yml`)

**Trigger**: Automatically on push to master/main when `.csproj` files are modified

**What it does**:
- Detects version changes in PackageVersion properties
- Outputs changed packages and version numbers
- Triggers changelog generation if changes detected

### 2. Manual Version Bump (`version-bump-release.yml`)

**Trigger**: Manual dispatch from GitHub Actions

**Steps**:
1. Go to Actions → "Automated Version Bump and Release"
2. Click "Run workflow"
3. Select bump type: `major`, `minor`, or `patch`
4. Select package: `YC.Monad`, `YC.Monad.EntityFrameworkCore`, or `Both`

**What it does**:
- Updates PackageVersion in selected projects
- Updates CHANGELOG.md with new section
- Creates git commit with version bump
- Creates and pushes git tag (e.g., `v1.4.0`)

### 3. Changelog Generation (`generate-changelog.yml`)

**Trigger**: After detect-version-changes.yml completes

**What it does**:
- Parses git log since last tag
- Extracts conventional commits (feat:, fix:, BREAKING CHANGE:)
- Updates CHANGELOG.md with new version section
- Commits and pushes changes

### 4. Package Publishing (`publish-packages.yml`)

**Trigger**: Manual dispatch OR automatic on git tag push (v*)

**Manual Publishing**:
1. Go to Actions → "Publish NuGet Packages"
2. Click "Run workflow"
3. Select which packages to publish
4. Enter version number

**Automatic Publishing**:
- When a git tag matching `v*` is pushed, automatically builds and publishes

**What it does**:
- Builds projects in Release configuration
- Runs all tests
- Packs NuGet packages (`.nupkg`)
- Pushes to NuGet.org
- Creates GitHub Release with artifacts

### 5. Build and Test (`ci-build-test.yml`)

**Trigger**: On every push and pull request

**What it does**:
- Builds against .NET 6.0, 7.0, and 8.0
- Runs all unit tests
- Validates semantic versioning format

## Setup Instructions

### 1. GitHub Secrets

Configure these secrets in your GitHub repository (Settings → Secrets and variables → Actions):

#### NUGET_API_KEY
- **Description**: API key for publishing to NuGet.org
- **How to get it**:
  1. Create NuGet account at https://www.nuget.org
  2. Go to Account Settings → API Keys
  3. Create new key with push privilege
  4. Add it as `NUGET_API_KEY` secret

### 2. Initial Setup

1. **Clone and enter repository**
   ```powershell
   git clone https://github.com/yusuf-cirak/YC.Solution.git
   cd YC.Solution
   ```

2. **Ensure .github directory exists**
   ```powershell
   mkdir -Force .github/workflows
   ```

3. **Create initial tags** (if not present)
   ```powershell
   git tag v1.3.0
   git push --tags
   ```

### 3. Local Version Management Script (PowerShell)

Use `scripts/version-sync.ps1` to check and update versions locally:

```powershell
# Check current versions
.\scripts\version-sync.ps1 -Action Check

# Update version locally (before committing)
.\scripts\version-sync.ps1 -Action Update -Package "YC.Monad" -Version "1.4.0"
```

## Workflow: Making a Release

### Option A: Automated Version Bump (Recommended)

1. Make your changes and commit with conventional commits:
   ```
   feat(monad): add new extension method GetValueOrThrow
   fix(efcore): handle null DbSet correctly
   ```

2. Go to GitHub Actions → "Automated Version Bump and Release"

3. Click "Run workflow" and select:
   - Bump type (patch/minor/major)
   - Package to bump

4. Workflow automatically:
   - Updates version numbers
   - Updates CHANGELOG.md
   - Creates git tag
   - Builds and publishes to NuGet
   - Creates GitHub Release

### Option B: Manual Version Update

1. Update `PackageVersion` in `.csproj` files:
   ```xml
   <PackageVersion>1.4.0</PackageVersion>
   ```

2. Commit with message:
   ```
   chore: bump version to 1.4.0
   ```

3. Manually trigger version detection and publishing workflows

## CHANGELOG.md Format

The project uses [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) format:

```markdown
## [1.4.0] - 2024-01-20

### YC.Monad

#### Added
- New GetValueOrThrow() extension method

#### Fixed
- Fixed null reference in Map() method

#### Changed
- Improved performance of Option caching

### YC.Monad.EntityFrameworkCore

#### Fixed
- Corrected handling of null queryables

---

[1.4.0]: https://github.com/yusuf-cirak/YC.Solution/compare/v1.3.0...v1.4.0
```

## Scripts

### scripts/version-sync.ps1

Local utility to sync versions:

```powershell
# List all packages and their current versions
.\scripts\version-sync.ps1 -Action Check

# Update a specific package version
.\scripts\version-sync.ps1 -Action Update -Package "YC.Monad" -Version "1.4.0"

# Validate all versions follow semantic versioning
.\scripts\version-sync.ps1 -Action Validate
```

## Troubleshooting

### Published version doesn't appear on NuGet

1. Check NUGET_API_KEY secret is set correctly
2. Verify version number doesn't already exist on NuGet
3. Check workflow run logs for push errors

### Changelog not updating

1. Ensure commits follow Conventional Commits format
2. Check changelog generation workflow logs
3. Verify git history is available (fetch-depth: 0)

### Version detection not triggering

1. Ensure changes are to `.csproj` files in `src/` directory
2. Push to `master` or `main` branch
3. Manually trigger workflow from GitHub Actions

## Related Files

- `.github/version-config.json` - Package configuration
- `.github/workflows/` - All automation workflows
- `CHANGELOG.md` - Release notes
- `src/**/YC.*.csproj` - Package project files (version source of truth)

## Best Practices

1. **Always use Conventional Commits** - Enables consistent changelog generation
2. **Use GitHub Actions** - Manual version bumps can cause discrepancies
3. **Review CHANGELOG.md** - After automatic generation, review for accuracy
4. **Tag releases** - Always push tags after version bump
5. **Test before publishing** - CI pipeline tests automatically, but verify locally first

