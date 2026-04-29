# Setup Branch Protection Workflow

## Overview

This workflow sets up GitHub branch protection rules to ensure CI checks pass before pull requests can be merged. It prevents merging code that hasn't been properly tested.

## Triggers

- **Manual**: Triggered manually from GitHub Actions UI only

## What It Does

1. **Configure Protection Rules**: Sets up required status checks and review requirements
2. **Apply to Repository**: Uses GitHub API to update branch protection settings
3. **Report Results**: Logs applied protection rules

## Required Secrets

None - uses GitHub's built-in token (`GITHUB_TOKEN`)

## Manual Inputs

- `repository`: Repository in format `owner/repo`
- `branch`: Branch to protect (default: `main`)
- `require_reviews`: Require pull request reviews (default: `true`)
- `required_review_count`: Number of required approving reviews (default: `1`)
- `require_up_to_date`: Require branches to be up to date (default: `true`)
- `include_admins`: Include administrators in protection rules (default: `true`)

## Protection Rules Applied

When enabled, the workflow applies:

### Status Checks
- **Required checks**: `ci-build-test`, `detect-version-changes`
- **Strict checks**: Branches must be up to date before merging
- **Include admins**: Protection applies to repository administrators

### Pull Request Reviews
- **Required reviews**: Specified number of approvals needed
- **Dismiss stale reviews**: When new commits are pushed

## Troubleshooting

### Permission Denied
- Ensure workflow has permission to modify repository settings
- Check that the repository owner/name is correct
- Verify the branch exists

### API Errors
- Check GitHub API rate limits
- Ensure repository is not archived
- Verify branch protection is not managed by organization rules

### Rules Not Applied
- Check that inputs are valid (boolean values, correct repository format)
- Review workflow logs for specific error messages
- Ensure the branch exists and is not already protected

## Manual Trigger

To setup branch protection:
1. Go to **Actions** tab
2. Select **"Setup Branch Protection"**
3. Click **"Run workflow"**
4. Configure inputs:
   - **Repository**: `your-username/YC.Solution`
   - **Branch**: `main` (or your default branch)
   - **Require reviews**: `true`
   - **Required review count**: `1`
   - **Require up to date**: `true`
   - **Include admins**: `true`
5. Click **"Run workflow"**

## Alternative Methods

### Local Script (if available)
```powershell
# Requires GitHub CLI
gh auth login
.\scripts\setup-branch-protection.ps1 -Repository "owner/repo" -RequireReviews
```

### Manual Setup via GitHub UI
1. Go to **Settings** → **Branches** → **Add rule**
2. Configure protection rules manually

## Verification

After running, verify protection is active:
1. Go to **Settings** → **Branches**
2. Check that your branch has protection rules
3. Try creating a PR to test the rules

## Related Files

- None - this workflow configures repository settings directly</content>
<parameter name="filePath">D:\Yazılım\csharp\repos\YC.Solution\.github\workflows\setup-branch-protection.yml.md
