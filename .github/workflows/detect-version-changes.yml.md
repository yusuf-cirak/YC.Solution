# Detect Version Changes Workflow

## Overview

This workflow automatically detects when package versions have been changed in `.csproj` files. It triggers downstream workflows for changelog generation and publishing when version changes are detected.

## Triggers

- **Push**: Any push to `master` or `main` branches that modifies `src/**/*.csproj` files
- **Manual**: Can be triggered manually from GitHub Actions UI

## What It Does

1. **Load Configuration**: Reads `.github/version-config.json`
2. **Check Version Changes**: Compares current vs previous versions in `.csproj` files
3. **Extract Package Info**: Gets package names and version numbers
4. **Set Outputs**: Provides changed packages data to other workflows
5. **Report Changes**: Logs detected version changes

## Required Secrets

None - this workflow doesn't require any secrets.

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

## Outputs

- `version_changed`: `true` if any versions changed
- `packages`: JSON array of changed packages with old/new versions

## Troubleshooting

### No Version Changes Detected
- Ensure changes are pushed to correct branch (`master`/`main`)
- Verify `.csproj` files are in `src/` directory
- Check that `PackageVersion` property was actually modified

### Workflow Doesn't Trigger
- Confirm file paths match the trigger pattern: `src/**/*.csproj`
- Check branch name matches trigger branches
- Manually trigger if needed

### Configuration Issues
- Validate `.github/version-config.json` syntax
- Ensure `csprojPath` values are correct relative paths

## Manual Trigger

To run manually:
1. Go to **Actions** tab
2. Select **"Detect Version Changes"**
3. Click **"Run workflow"**
4. Select branch (optional)

## Related Files

- `.github/version-config.json` - Package configuration
- `src/**/*.csproj` - Project files with version info
- `generate-changelog.yml` - Triggered by this workflow</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\detect-version-changes.yml.md
