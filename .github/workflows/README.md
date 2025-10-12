# GitHub Actions Workflows

This repository uses streamlined GitHub Actions workflows to minimize server load and provide clear control over when builds run.

## 📦 Production Release Workflow

### `dual-release.yml`
**Trigger:** Automatic when you push a version tag (e.g., `v10.3.0`)

**What it does:**
- Builds **both** Modern UI (from `main` branch) and Classic UI (from `classic-ui` branch)
- Creates 4 downloadable files:
  - `OSR-Studio-Modern-Setup.exe` (Modern UI installer)
  - `OSR-Studio-Modern-vX.X.X-Portable.zip` (Modern UI portable)
  - `OSR-Studio-Classic-Setup.exe` (Classic UI installer)
  - `OSR-Studio-Classic-vX.X.X-Portable.zip` (Classic UI portable)
- Publishes a GitHub Release with all 4 files

**When to use:**
```bash
# After updating version numbers on both branches
git checkout main
git tag v10.3.0
git push origin v10.3.0
```

## 🐛 Debug Build Workflow

### `debug-builds.yml`
**Trigger:** Manual only (workflow_dispatch)

**What it does:**
- Builds Debug version of Modern UI, Classic UI, or both
- Uploads artifacts (available for 7 days)
- No release created, no installers built

**When to use:**
1. Go to **Actions** tab on GitHub
2. Click **Debug Builds** workflow
3. Click **Run workflow**
4. Choose which UI to build:
   - `modern` - Build only Modern UI
   - `classic` - Build only Classic UI  
   - `both` - Build both UIs
5. Click **Run workflow**

**Perfect for:**
- Testing bug fixes before release
- Verifying builds without creating releases
- Quick iteration during development

## 🎯 Workflow Design Principles

### Minimal Server Load
- **No automatic builds on push** - Prevents hundreds of workflow runs
- **No automatic builds on PR** - Only build when you explicitly need to test
- **Manual debug builds** - Full control over when to test
- **Release only on tags** - Production releases are intentional

### Clear Separation
- **Production** - Only `dual-release.yml` runs automatically (on tags)
- **Development** - `debug-builds.yml` runs manually when you need it
- **No redundancy** - Each workflow has a single, clear purpose

## 🧹 Streamlined Workflow Structure

This repository now uses **3 optimized workflows** (down from 8):

1. **`dual-release.yml`** - Production releases (automatic on tags)
2. **`debug-builds.yml`** - Manual testing (full control)
3. **`test-cursor-branches.yml`** - Auto-test Cursor branches only

**Removed obsolete workflows:**
- ❌ `build.yml` (replaced by dual-release)
- ❌ `build-dotnet-desktop-pull-request.yml` (old template)
- ❌ `release-build-dotnet-desktop.yml` (replaced by dual-release)
- ❌ `test-modern-ui.yml` (ran on every push - excessive)
- ❌ `test-classic-ui.yml` (merged into dual-release)

## 🚀 Usage Examples

### Example 1: Regular Development
```bash
# Make changes
git commit -m "Fix bug"
git push

# ✅ No workflows run - no server load
```

### Example 2: Test Before Release
1. Push your changes
2. Go to Actions → Debug Builds → Run workflow
3. Select "both" to test both UIs
4. Download artifacts and test locally
5. If good, create release tag

### Example 3: Create Release
```bash
# Update versions on both branches
git checkout main
# Edit version to 10.3.1
git commit -m "Bump to 10.3.1"
git push

git checkout classic-ui
# Edit version to 10.3.1
git commit -m "Bump to 10.3.1"
git push

# Create release
git checkout main
git tag v10.3.1
git push origin v10.3.1

# ✅ dual-release.yml runs automatically
# ✅ Creates release with 4 files
```

## 🛠️ Maintenance

### If You Need to Modify Workflows

**For debug builds:**
Edit `.github/workflows/debug-builds.yml`

**For releases:**
Edit `.github/workflows/dual-release.yml`

**Testing workflow changes:**
Use `debug-builds.yml` to test - it won't create releases even if it fails.

## 📝 Notes

- Debug artifacts are kept for **7 days** only
- Releases are permanent
- All builds use **windows-2022** runner
- Debug builds use **Debug** configuration
- Release builds use **Release** configuration

---

**Questions?** Check the workflow files themselves - they have detailed comments!
