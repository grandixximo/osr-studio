# Version Bumping Guide

This guide explains how to properly bump the version number for OSR Studio releases.

## Overview

OSR Studio uses a dual-branch system:
- **main** - Modern UI version
- **classic-ui** - Classic UI version

Both branches share the same version numbers and are released simultaneously.

## Files to Update

When bumping the version, you need to update these files:

### 1. Assembly Version Files
- `src/OsrStudio/Properties/AssemblyInfo.cs`
- `src/OsrStudio.Console/Properties/AssemblyInfo.cs`

Update the `AssemblyVersion` attribute:
```csharp
[assembly: AssemblyVersion("10.7.1")]
```

### 2. Changelog Files
- `docs/Changelogs/main.json` - For main branch
- `docs/Changelogs/classic-ui.json` - For classic-ui branch

Add a new release entry at the beginning of the `releases` array. See `docs/Changelogs/HOWTO.md` for details.

## Version Bumping Process

### Step 1: Update Main Branch

```bash
# Make sure you're on main
git checkout main

# Update version in assembly files
# Edit: src/OsrStudio/Properties/AssemblyInfo.cs
# Edit: src/OsrStudio.Console/Properties/AssemblyInfo.cs

# Update changelog
# Edit: docs/Changelogs/main.json

# Commit changes
git add src/OsrStudio/Properties/AssemblyInfo.cs src/OsrStudio.Console/Properties/AssemblyInfo.cs docs/Changelogs/main.json
git commit -m "Bump version to X.Y.Z and update changelog"

# Push to GitHub
git push origin main
```

### Step 2: Update Classic-UI Branch

```bash
# Switch to classic-ui
git checkout classic-ui

# Merge main to get version updates
git merge main

# Update classic-ui changelog
# Edit: docs/Changelogs/classic-ui.json

# Commit changes
git add docs/Changelogs/classic-ui.json
git commit -m "Update classic-ui changelog for X.Y.Z"

# Push to GitHub
git push origin classic-ui
```

### Step 3: Create Release Tag

**Important**: Only create ONE tag on the main branch. The GitHub Actions workflow will automatically build both branches.

```bash
# Switch back to main
git checkout main

# Create annotated tag
git tag -a vX.Y.Z -m "OSR Studio vX.Y.Z - Description"

# Push tag to GitHub
git push origin vX.Y.Z
```

## Release Workflow

When you push a tag to the main branch:

1. **GitHub Actions detects the tag**
2. **Builds main branch** → OSR Studio (Modern UI)
3. **Automatically checks out classic-ui** → OSR Studio Classic UI
4. **Creates a single GitHub Release** with both installers

### Why Only One Tag?

- The GitHub Actions workflow (`.github/workflows/dual-release.yml`) is designed to build both branches from a single tag
- This ensures both versions have the same version number
- It creates one unified release with both UI variants
- No need for separate `-classic` tags

## Version Number Format

OSR Studio follows semantic versioning:
- **Major.Minor.Patch** (e.g., `10.7.1`)
- **Major**: Breaking changes or major features
- **Minor**: New features, backward compatible
- **Patch**: Bug fixes and minor improvements

## Checklist

Before releasing:
- [ ] Version updated in both AssemblyInfo.cs files
- [ ] Changelog updated in main.json
- [ ] Changes committed and pushed to main
- [ ] Classic-ui merged from main
- [ ] Changelog updated in classic-ui.json
- [ ] Changes committed and pushed to classic-ui
- [ ] Tag created on main branch only
- [ ] Tag pushed to GitHub

## Troubleshooting

**Both versions not building?**
- Check that the GitHub Actions workflow is enabled
- Verify the tag was created on the main branch
- Check Actions tab for build errors

**Version mismatch between branches?**
- Always merge main into classic-ui after version bump
- Both assembly files should have identical version numbers

**Tag already exists?**
- Delete local tag: `git tag -d vX.Y.Z`
- Delete remote tag: `git push origin :refs/tags/vX.Y.Z`
- Recreate the tag on the correct commit

## Example: Bumping to 10.7.1

```bash
# Main branch
git checkout main
# Edit version files and changelog
git add src/OsrStudio/Properties/AssemblyInfo.cs src/OsrStudio.Console/Properties/AssemblyInfo.cs docs/Changelogs/main.json
git commit -m "Bump version to 10.7.1 and update changelog"
git push origin main

# Classic-UI branch
git checkout classic-ui
git merge main
# Edit classic-ui changelog
git add docs/Changelogs/classic-ui.json
git commit -m "Update classic-ui changelog for 10.7.1"
git push origin classic-ui

# Create release tag on main
git checkout main
git tag -a v10.7.1 -m "OSR Studio v10.7.1 - Bug Fixes"
git push origin v10.7.1

# GitHub Actions will now build both branches automatically!
```

## See Also

- [Changelog Guide](Changelogs/HOWTO.md)
- [GitHub Actions Workflow](../.github/workflows/dual-release.yml)
- [Release Process](README.md)

