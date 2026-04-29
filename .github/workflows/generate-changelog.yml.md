# Generate Changelog Workflow

## Overview

This workflow automatically generates and updates `CHANGELOG.md` following the Keep a Changelog format. It parses git commits since the last tag and categorizes them based on Conventional Commits.

## Triggers

- **Automatic**: Triggered by `detect-version-changes.yml` when version changes are detected
- **Manual**: Can be triggered manually from GitHub Actions UI

## What It Does

1. **Parse Git History**: Analyzes commits since last version tag
2. **Extract Conventional Commits**: Identifies `feat:`, `fix:`, `BREAKING CHANGE:`, etc.
3. **Categorize Changes**: Groups by package and change type
4. **Update CHANGELOG.md**: Adds new version section with formatted entries
5. **Commit Changes**: Automatically commits and pushes changelog updates

## Required Secrets

None - this workflow doesn't require any secrets.

## Configuration

Depends on:
- Conventional Commits in git history
- `.github/version-config.json` for package names
- Existing `CHANGELOG.md` structure

## Commit Types Recognized

- `feat:` → Added section
- `fix:` → Fixed section
- `BREAKING CHANGE:` → Breaking Changes section
- `perf:` → Changed section
- `refactor:` → Changed section
- `docs:` → Changed section
- `style:` → Changed section
- `test:` → Changed section
- `chore:` → Changed section
- `ci:` → Changed section

## Troubleshooting

### Changelog Not Generated
- Ensure commits follow Conventional Commits format
- Check that version changes were detected first
- Verify git history is accessible (`fetch-depth: 0`)

### Incorrect Categorization
- Review commit messages for proper conventional commit format
- Check for typos in commit type keywords
- Ensure breaking changes include `BREAKING CHANGE:` footer

### Missing Package Sections
- Verify `.github/version-config.json` has correct package names
- Check commit scopes match package names

## Manual Trigger

To run manually:
1. Go to **Actions** tab
2. Select **"Generate Changelog"**
3. Click **"Run workflow"**
4. Select branch (optional)

## Example Output

```markdown
## [1.4.0] - 2024-01-20

### YC.Monad

#### Added
- New GetValueOrThrow() extension method

#### Fixed
- Fixed null reference in Map() method

#### Changed
- Improved performance of Option caching
```

## Related Files

- `CHANGELOG.md` - The changelog file being updated
- `.github/version-config.json` - Package configuration
- `detect-version-changes.yml` - Triggers this workflow</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\generate-changelog.yml.md
