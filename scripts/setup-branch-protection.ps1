# Branch Protection Setup Script
# This script configures GitHub branch protection rules to ensure
# CI checks pass before PRs can be merged

param(
    [Parameter(Mandatory=$true)]
    [string]$Repository,

    [Parameter(Mandatory=$false)]
    [string]$Branch = "main",

    [switch]$RequireReviews,
    [int]$RequiredReviewCount = 1
)

Write-Host "🔒 Setting up branch protection for $Repository on branch '$Branch'" -ForegroundColor Cyan

# Check if GitHub CLI is installed
if (!(Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "❌ GitHub CLI (gh) is not installed. Please install it first:" -ForegroundColor Red
    Write-Host "   https://cli.github.com/" -ForegroundColor Yellow
    exit 1
}

# Check if authenticated
$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Not authenticated with GitHub CLI. Please run 'gh auth login' first." -ForegroundColor Red
    exit 1
}

Write-Host "✅ GitHub CLI authenticated" -ForegroundColor Green

# Build the branch protection command
$cmd = "gh api repos/$Repository/branches/$Branch/protection --method PUT --field required_status_checks='{""strict"":true,""contexts"":[""ci-build-test"",""detect-version-changes""]}'"

if ($RequireReviews) {
    $cmd += " --field required_pull_request_reviews='{""required_approving_review_count"":$RequiredReviewCount}'"
}

$cmd += " --field enforce_admins=true"
$cmd += " --field restrictions=null"

Write-Host "🔧 Executing: $cmd" -ForegroundColor Yellow

try {
    $result = Invoke-Expression $cmd
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Branch protection rules applied successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "📋 Applied rules:" -ForegroundColor Cyan
        Write-Host "   • Require status checks to pass: ci-build-test, detect-version-changes"
        Write-Host "   • Require branches to be up to date before merging"
        if ($RequireReviews) {
            Write-Host "   • Require $RequiredReviewCount approving review(s)"
        }
        Write-Host "   • Include administrators"
    } else {
        Write-Host "❌ Failed to apply branch protection rules" -ForegroundColor Red
        Write-Host "Error: $result" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ Error applying branch protection: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🎉 Setup complete! PRs can now only be merged after CI checks pass." -ForegroundColor Green
