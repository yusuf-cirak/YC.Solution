# GitHub Actions - NuGet Publishing Workflow Setup Guide

## Overview

This workflow automates the publishing of multiple NuGet packages from your mono-repo. It includes:
- ✅ Automatic change detection per project
- ✅ Version validation (prevents version regression)
- ✅ Parallel build & test matrix
- ✅ SonarQube quality gate enforcement
- ✅ NuGet publishing with retry logic
- ✅ Safety guards (concurrency control, main branch only)

## Files Created

### 1. `.github/workflows/release.yml`
The main workflow file. Triggers on every push to `main` branch.

### 2. `projects.json`
Configuration file listing all publishable NuGet packages with:
- Package name
- Project directory path
- .csproj file location
- Associated test project path

## Setup Instructions

### Step 1: Add GitHub Secrets

Go to **Settings → Secrets and variables → Actions** and add:

#### Required:
- **`NUGET_API_KEY`**: Your NuGet API key from https://www.nuget.org/account/ApiKeys

#### Optional (for SonarQube):
- **`SONAR_TOKEN`**: Your SonarQube/SonarCloud token
- **`SONAR_HOST_URL`**: Your SonarQube server URL (defaults to https://sonarcloud.io)

If `SONAR_TOKEN` is not set, SonarQube analysis is skipped automatically.

### Step 2: Customize projects.json

If you add new packages, update `projects.json`:

```json
{
  "projects": [
    {
      "name": "MyPackage.Name",
      "path": "src/MyPackage.Name",
      "csproj": "src/MyPackage.Name/MyPackage.Name.csproj",
      "testProject": "test/MyPackage.Name.UnitTests/MyPackage.Name.UnitTests.csproj"
    }
  ]
}
```

### Step 3: Version Your Packages

Each package version is defined in its `.csproj` file using `<PackageVersion>`:

```xml
<PropertyGroup>
  <PackageVersion>1.2.3</PackageVersion>
</PropertyGroup>
```

## Workflow Behavior

### 1️⃣ **Change Detection**
- Compares current commit with previous commit
- Detects file changes within each project directory
- Only processes changed projects (efficiency)

### 2️⃣ **Version Validation**
Per project that changed:
- **Extracts** `<PackageVersion>` from `.csproj`
- **Queries** NuGet.org for latest published version
- **Validates**:
  - ❌ Version decreased → **FAILS pipeline**
  - ⚠️ Version unchanged → **SKIPS publish** for that package
  - ✅ Version increased → **PROCEEDS to build**

### 3️⃣ **Build & Test** (Parallel Matrix)
For each changed project:
- Restores NuGet packages (cached)
- Builds in Release mode
- Runs associated unit tests
- Fails fast on any error

### 4️⃣ **SonarQube Analysis** (Optional)
If `SONAR_TOKEN` is configured:
- Runs SonarScanner for each project
- Enforces Quality Gate
- Fails if Quality Gate doesn't pass
- Skips entire job if token not set

### 5️⃣ **NuGet Publish** (With Retries)
For each changed project with version increase:
1. Packs the project into `.nupkg`
2. Pushes to NuGet.org with `--skip-duplicate`
3. **Retries up to 3 times** if push fails (handles transient network issues)
4. Verifies package appears on NuGet.org

## Safety Features

### Concurrency Control
```yaml
concurrency:
  group: release-${{ github.ref }}
  cancel-in-progress: false
```
- Only one release workflow runs at a time per branch
- Prevents duplicate publishes to NuGet

### Main Branch Only
```yaml
on:
  push:
    branches:
      - main
```
- Workflow only triggers on pushes to `main`
- Prevents accidental publishes from feature branches

### Version Validation
- Detects version decreases and fails immediately
- Queries NuGet to prevent re-publishing same version
- Handles first-time package publication (compares to 0.0.0)

## Caching

The workflow caches NuGet packages at `~/.nuget/packages`:
```yaml
key: nuget-${{ runner.os }}-${{ hashFiles('**/packages.lock.json', '**/Directory.Packages.props') }}
```
- Significantly speeds up `dotnet restore`
- Falls back if cache key doesn't match

## Monitoring & Troubleshooting

### View Workflow Runs
1. Go to **Actions** tab in your repository
2. Select **Release - Publish NuGet Packages**
3. Click any run to see detailed logs

### Common Issues

#### ❌ "Version decreased! This is not allowed."
- Check your `.csproj` `<PackageVersion>` tag
- Ensure it's higher than the version on NuGet.org
- Push must be to `main` branch

#### ❌ "Failed to publish package after 3 attempts"
- Check `NUGET_API_KEY` secret is correct
- Verify package name matches NuGet.org exactly
- Ensure your API key has publish permissions

#### ⚠️ "No .nupkg file found!"
- Check project builds successfully locally: `dotnet build --configuration Release`
- Verify `.csproj` has `<IsPackable>true</IsPackable>` (defaults to true for libraries)

#### ⏭️ "Quality Gate failed!"
- Check SonarQube analysis logs
- Address quality issues before pushing to main
- Ensure `SONAR_TOKEN` has correct permissions

### Debug Tips

1. **Check changed file detection**:
   - Look for "Changed files:" section in job logs
   - Verify files match project paths in `projects.json`

2. **Check version extraction**:
   - Look for "Version in .csproj:" in validate-versions job
   - Grep command requires exact XML format

3. **Enable detailed test output**:
   - Already configured with `--verbosity normal`
   - Check test job logs if tests fail

## Example Scenarios

### Scenario 1: Publish One Package Only
```
- Edit src/YC.Monad/YC.Monad.csproj
- Change <PackageVersion>1.3.0</PackageVersion> to <PackageVersion>1.3.1</PackageVersion>
- Push to main
→ Only YC.Monad is detected and processed
→ YC.Monad.EntityFrameworkCore skips all jobs
```

### Scenario 2: Publish Two Packages
```
- Edit src/YC.Monad/YC.Monad.csproj              (v1.3.0 → v1.3.1)
- Edit src/YC.Monad.EntityFrameworkCore/YC.Monad.EntityFrameworkCore.csproj  (v1.0.0 → v1.1.0)
- Push to main
→ Both projects detected and processed in parallel
→ Both published independently
```

### Scenario 3: Version Not Increased
```
- Edit src/YC.Monad/SomeFile.cs (only)
- Don't change <PackageVersion>
- Push to main
→ Project detected but version validation shows no increase
→ Publish is skipped for that package
→ Pipeline succeeds (no error)
```

### Scenario 4: Version Decreased (Should Fail)
```
- Change <PackageVersion>1.3.0</PackageVersion> to <PackageVersion>1.2.0</PackageVersion>
- Push to main
→ Version validation fails immediately
→ Pipeline fails (not published)
→ Error message indicates version decrease not allowed
```

## Performance Notes

- **Caching**: NuGet restore cached, typically 2-5x faster
- **Parallel builds**: All changed projects build simultaneously
- **Selective testing**: Only changed projects tested
- **First run**: ~2-3 minutes (includes SonarQube if enabled)
- **Subsequent runs**: ~1-2 minutes (with cache hits)

## Next Steps

1. ✅ Push `projects.json` and `.github/workflows/release.yml` to repository
2. ✅ Add `NUGET_API_KEY` secret to GitHub (required)
3. ✅ Optionally add `SONAR_TOKEN` for quality gates
4. ✅ Test with a version bump to any package
5. ✅ Monitor first run in Actions tab

## Additional Customization

### To Run on Other Branches
Change line 5 in `release.yml`:
```yaml
on:
  push:
    branches:
      - main
      - master        # Add other branches
```

### To Add More Projects
1. Update `projects.json`:
   ```json
   {
     "name": "NewPackage",
     "path": "src/NewPackage",
     "csproj": "src/NewPackage/NewPackage.csproj",
     "testProject": "test/NewPackage.UnitTests/NewPackage.UnitTests.csproj"
   }
   ```
2. Ensure test project exists and uses xUnit/NUnit

### To Disable SonarQube
- Simply don't set `SONAR_TOKEN` secret
- Job automatically skips

### To Use Private NuGet Feed
Replace in publish step:
```yaml
--source https://your-nuget-server/v3/index.json
```

## Support

For issues or questions:
1. Check GitHub Actions logs for detailed error messages
2. Review version validation rules above
3. Ensure all secrets are correctly configured
4. Verify `.csproj` files have proper `<PackageVersion>` tags


