# Automated Version Bump and Release Workflow

## Overview

This workflow provides one-click version bumping with automatic changelog generation, package publishing, and GitHub release creation. It's the primary workflow for releasing new versions.

## Triggers

- **Manual**: Triggered manually from GitHub Actions UI only

## What It Does

1. **Version Bump**: Updates `PackageVersion` in selected `.csproj` files
2. **Changelog Update**: Generates new changelog section from conventional commits
3. **Git Operations**: Commits changes and creates version tag
4. **Build & Test**: Validates the build after version changes
5. **Publish Packages**: Pushes to NuGet.org
6. **Create Release**: Generates GitHub release with changelog

## Required Secrets

- `NUGET_API_KEY`: API key for NuGet.org publishing
  - Get from: https://www.nuget.org/account/apikeys
  - Must have "Push" permission

## Manual Inputs

- `bump_type`: Type of version bump (`patch`, `minor`, `major`)
- `package_to_bump`: Which package to bump (`YC.Monad`, `YC.Monad.EntityFrameworkCore`, `Both`)

## Version Bump Logic

- **Patch** (`0.0.X`): Bug fixes, small changes
- **Minor** (`0.X.0`): New features, backwards compatible
- **Major** (`X.0.0`): Breaking changes

## Workflow Steps

1. **Determine New Version**: Calculates next version based on current + bump type
2. **Update Projects**: Modifies `PackageVersion` in `.csproj` files
3. **Generate Changelog**: Parses commits since last tag
4. **Commit & Tag**: Creates commit and git tag (e.g., `v1.4.0`)
5. **Build Validation**: Ensures everything compiles
6. **Publish**: Pushes packages to NuGet.org
7. **Release**: Creates GitHub release

## Troubleshooting

### Version Calculation Issues
- Check current version format in `.csproj` files
- Ensure semantic versioning (X.Y.Z) is used
- Verify bump type selection

### Changelog Generation Fails
- Ensure recent commits follow Conventional Commits format
- Check git history is accessible
- Verify package names match configuration

### Publishing Fails
- Verify `NUGET_API_KEY` secret is set
- Check API key permissions
- Ensure version doesn't already exist on NuGet

### Git Operations Fail
- Check repository permissions for tagging
- Ensure branch is up to date
- Verify no conflicts with existing tags

## Manual Trigger

To create a new release:
1. Go to **Actions** tab
2. Select **"Automated Version Bump and Release"**
3. Click **"Run workflow"**
4. Configure inputs:
   - **Bump type**: Select `patch`, `minor`, or `major`
   - **Package to bump**: Select package(s) to release
5. Click **"Run workflow"**

## Example Usage

**Patch Release** (1.3.0 → 1.3.1):
- For bug fixes
- Backwards compatible
- No new features

**Minor Release** (1.3.0 → 1.4.0):
- For new features
- Backwards compatible
- May include bug fixes

**Major Release** (1.3.0 → 2.0.0):
- For breaking changes
- API changes
- Major rewrites

## Outputs

- Updated `CHANGELOG.md` with new version section
- Git tag (e.g., `v1.4.0`)
- NuGet packages published
- GitHub release with changelog and download links

## Related Files

- `.github/version-config.json` - Package configuration
- `src/**/*.csproj` - Projects with version info
- `CHANGELOG.md` - Changelog being updated
- `Directory.Build.props` - Build configuration</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\version-bump-release.yml.md
