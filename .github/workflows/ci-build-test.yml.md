# CI Build & Test Workflow

## Overview

This workflow runs automated build and test validation for all .NET projects in the solution. It ensures code quality and compatibility across multiple .NET versions.

## Triggers

- **Push**: Any push to `master` or `main` branches
- **Pull Request**: Any PR targeting `master` or `main` branches
- **Manual**: Can be triggered manually from GitHub Actions UI

## What It Does

1. **Setup .NET SDKs**: Installs .NET 6.0, 7.0, and 8.0
2. **Restore Dependencies**: Runs `dotnet restore`
3. **Build**: Compiles all projects in Release configuration
4. **Test**: Executes all unit tests
5. **Validation**: Checks semantic versioning format

## Required Secrets

None - this workflow doesn't require any secrets.

## Configuration

The workflow uses the following .NET version:
- .NET 6.0

## Outputs

- **Build Status**: Success/failure of compilation
- **Test Results**: Pass/fail status of unit tests
- **Test Coverage**: (if configured)

## Troubleshooting

### Build Fails
- Check .NET SDK compatibility
- Verify `Directory.Build.props` and `Directory.Packages.props`
- Ensure all NuGet packages are available

### Tests Fail
- Check test project configurations
- Verify test data and mock setups
- Review test logs for specific failures

### Semantic Version Validation Fails
- Ensure `PackageVersion` follows X.Y.Z format
- Check for invalid characters in version strings

## Manual Trigger

To run manually:
1. Go to **Actions** tab
2. Select **"CI Build & Test"**
3. Click **"Run workflow"**
4. Select branch (optional)

## Related Files

- `YC.Solution.slnx` - Solution file
- `Directory.Build.props` - Build configuration
- `Directory.Packages.props` - Package references
- `test/**/*.csproj` - Test projects</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\ci-build-test.yml.md
