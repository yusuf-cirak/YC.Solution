# Publish NuGet Packages Workflow

## Overview

This workflow builds, tests, and publishes NuGet packages to NuGet.org. It can be triggered automatically on version tags or manually for specific package publishing.

## Triggers

- **Automatic**: When a git tag matching `v*` is pushed (e.g., `v1.4.0`)
- **Manual**: Can be triggered manually from GitHub Actions UI

## What It Does

1. **Setup .NET**: Installs required .NET SDK versions
2. **Restore & Build**: Compiles all projects in Release configuration
3. **Run Tests**: Executes full test suite
4. **Pack Packages**: Creates `.nupkg` files for configured packages
5. **Publish to NuGet**: Pushes packages to NuGet.org
6. **Create GitHub Release**: Generates release with changelog and artifacts

## Required Secrets

- `NUGET_API_KEY`: API key for NuGet.org publishing
  - Get from: https://www.nuget.org/account/apikeys
  - Must have "Push" permission

## Manual Inputs

When triggered manually:
- `package_to_publish`: Which package to publish (`YC.Monad`, `YC.Monad.EntityFrameworkCore`, or `Both`)
- `version`: Version number to publish (e.g., `1.4.0`)

## Configuration

Depends on `.github/version-config.json`:

```json
{
  "packages": [
    {
      "name": "YC.Monad",
      "csprojPath": "src/YC.Monad/YC.Monad.csproj"
    }
  ]
}
```

## Troubleshooting

### Publishing Fails
- Verify `NUGET_API_KEY` secret is set correctly
- Check API key has push permissions
- Ensure version doesn't already exist on NuGet.org
- Confirm package ownership on NuGet.org

### Build/Test Fails
- Check .NET SDK compatibility
- Verify all dependencies are available
- Review test failures in logs

### GitHub Release Not Created
- Ensure tag follows `v*` pattern
- Check workflow has permission to create releases
- Verify changelog exists and is properly formatted

## Manual Trigger

To publish manually:
1. Go to **Actions** tab
2. Select **"Publish NuGet Packages"**
3. Click **"Run workflow"**
4. Fill inputs:
   - **Package to publish**: Select package
   - **Version**: Enter version number
5. Click **"Run workflow"**

## Automatic Trigger

To trigger automatically:
1. Create and push a version tag:
   ```bash
   git tag v1.4.0
   git push --tags
   ```

## Outputs

- NuGet packages published to NuGet.org
- GitHub Release created with:
  - Changelog excerpt
  - Package download links
  - Release notes

## Related Files

- `.github/version-config.json` - Package configuration
- `src/**/*.csproj` - Projects to build and pack
- `CHANGELOG.md` - Used for release notes
- `Directory.Build.props` - Build configuration</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\publish-packages.yml.md
