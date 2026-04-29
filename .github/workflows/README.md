# GitHub Actions Workflows Documentation

This directory contains all GitHub Actions workflows for the YC.Solution project. Each workflow has its own documentation file.

## Workflows Overview

| Workflow | Purpose | Trigger | Documentation |
|----------|---------|---------|---------------|
| [`ci-build-test.yml`](ci-build-test.yml.md) | Build and test validation | Push/PR | [📖 Docs](ci-build-test.yml.md) |
| [`detect-version-changes.yml`](detect-version-changes.yml.md) | Detect package version changes | Push to .csproj | [📖 Docs](detect-version-changes.yml.md) |
| [`generate-changelog.yml`](generate-changelog.yml.md) | Auto-generate changelog | After version detection | [📖 Docs](generate-changelog.yml.md) |
| [`publish-packages.yml`](publish-packages.yml.md) | Publish to NuGet.org | Tag push / Manual | [📖 Docs](publish-packages.yml.md) |
| [`setup-branch-protection.yml`](setup-branch-protection.yml.md) | Configure branch protection | Manual | [📖 Docs](setup-branch-protection.yml.md) |
| [`version-bump-release.yml`](version-bump-release.yml.md) | One-click version bump & release | Manual | [📖 Docs](version-bump-release.yml.md) |
| [`version-management.yml`](version-management.yml.md) | Manual version management | Manual | [📖 Docs](version-management.yml.md) |

## Quick Start

### For Contributors
1. **CI/CD**: Automatic on every push/PR via `ci-build-test.yml`
2. **Branch Protection**: Setup via `setup-branch-protection.yml`

### For Maintainers
1. **Version Bump**: Use `version-bump-release.yml` for releases
2. **Manual Management**: Use `version-management.yml` for version tasks
3. **Publishing**: Use `publish-packages.yml` for manual publishing

## Workflow Categories

### 🔄 Automated (No Manual Intervention)
- `ci-build-test.yml` - Quality gates
- `detect-version-changes.yml` - Change detection
- `generate-changelog.yml` - Documentation

### 🎯 Manual Workflows
- `version-bump-release.yml` - **Primary release workflow**
- `version-management.yml` - Version utilities
- `setup-branch-protection.yml` - Repository setup
- `publish-packages.yml` - Manual publishing

## Required Secrets

| Secret | Used By | Purpose |
|--------|---------|---------|
| `NUGET_API_KEY` | `version-bump-release.yml`, `publish-packages.yml` | NuGet.org publishing |

## Common Tasks

### Create a New Release
1. Go to Actions → "Automated Version Bump and Release"
2. Select bump type (patch/minor/major)
3. Choose package(s)
4. Run workflow

### Check Current Versions
1. Go to Actions → "Version Management"
2. Action: `check`
3. Run workflow

### Setup Branch Protection
1. Go to Actions → "Setup Branch Protection"
2. Configure repository and rules
3. Run workflow

### Manual Package Publishing
1. Go to Actions → "Publish NuGet Packages"
2. Select package and version
3. Run workflow

## Troubleshooting

### Workflow Not Triggering
- Check trigger conditions in workflow file
- Verify branch names match
- Check file path patterns for push triggers

### Workflow Failing
- Check workflow logs in Actions tab
- Verify required secrets are set
- Check repository permissions

### Permission Issues
- Ensure workflows have necessary permissions
- Check branch protection settings
- Verify token scopes

## Related Documentation

- [Main README](../../README_PACKAGE_AUTOMATION.md) - Quick start guide
- [Version Management Guide](../../VERSION_MANAGEMENT_GUIDE.md) - Detailed documentation
- [Setup Checklist](../../SETUP_FINAL_CHECKLIST.md) - Implementation checklist

## Contributing

When adding new workflows:
1. Create the `.yml` file
2. Add corresponding `.md` documentation
3. Update this index file
4. Test the workflow thoroughly
5. Update main documentation files</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\README.md
