# YC.Solution - Package Automation Setup

## 🚀 Quick Start

Your repository now has automated package management! Here's what you need to do:

### 1. Push to GitHub
```powershell
git add .
git commit -m 'chore: add package automation workflows and update gitignore'
git push
```

### 2. Add NuGet API Key
- Go to **Settings** → **Secrets and variables** → **Actions**
- Add `NUGET_API_KEY` (get from https://www.nuget.org)

### 3. Setup Branch Protection (Important!)
To prevent PRs from merging before CI passes:

**Automated (Recommended):**
- Go to **Actions** → "Setup Branch Protection"
- Click "Run workflow"
- Fill in repository details

**Alternative (Local Script):**
```powershell
# Install GitHub CLI: https://cli.github.com/
gh auth login
.\scripts\setup-branch-protection.ps1 -Repository "your-username/YC.Solution" -RequireReviews
```

**Manual:**
- Go to **Settings** → **Branches** → **Add rule**
- Branch: `main`
- Require: `ci-build-test`, `detect-version-changes`
- Require PR reviews: 1

### 4. Test the Automation
- Go to **Actions** → "Automated Version Bump and Release"
- Click "Run workflow"
- Select bump type and package

## 📋 What's Included

### Workflows
- ✅ **ci-build-test.yml** - Build & test on every push/PR
- ✅ **detect-version-changes.yml** - Auto-detect version bumps
- ✅ **generate-changelog.yml** - Keep-a-Changelog format
- ✅ **publish-packages.yml** - NuGet publishing + GitHub releases
- ✅ **version-bump-release.yml** - One-click version bumps
- ✅ **setup-branch-protection.yml** - Automated branch protection setup
- ✅ **version-management.yml** - Manual version management (alternative to local script)

### Scripts
- ✅ **scripts/version-sync.ps1** - Local version management
- ✅ **scripts/setup-branch-protection.ps1** - Branch protection setup

### Config
- ✅ **.github/version-config.json** - Package metadata
- ✅ **CHANGELOG.md** - Release notes (Keep-a-Changelog)
- ✅ **VERSION_MANAGEMENT_GUIDE.md** - Detailed documentation

## 🎯 How It Works

1. **Make changes** with conventional commits (`feat:`, `fix:`, etc.)
2. **Version bump** via GitHub Actions (patch/minor/major)
3. **Auto-generate** changelog from commits
4. **Build & test** automatically
5. **Publish** to NuGet + create GitHub release

## 📖 Documentation

- **Full Guide**: `VERSION_MANAGEMENT_GUIDE.md`
- **Setup Details**: `PACKAGE_AUTOMATION_SETUP.md`
- **Checklist**: `SETUP_FINAL_CHECKLIST.md`

## 🔧 Tools & Management

### GitHub Actions (Recommended)
- **Version Management**: Actions → "Version Management" → Run workflow
- **Branch Protection**: Actions → "Setup Branch Protection" → Run workflow

### Local Scripts (Alternative)
```powershell
# Check versions
.\scripts\version-sync.ps1 -Action Check

# Update version
.\scripts\version-sync.ps1 -Action Update -Package "YC.Monad" -Version "1.4.0"

# Setup branch protection
.\scripts\setup-branch-protection.ps1 -Repository "your-username/YC.Solution" -RequireReviews
```

---

**Status**: ✅ Ready to push and use!
