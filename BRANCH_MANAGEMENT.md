# 🎯 BRANCH MANAGEMENT - OSR Studio

## Branch Structure

### `main` - Modern UI
- Modern, minimalist design (v9.0+)
- **Missing:** ConfigPage, ExtrasPage, CropWindow, FFmpegLogWindow, PreviewWindow, WebcamControl

### `classic-ui` - Classic UI  
- Original v8.0.0 design with vertical button layout
- **Has all modern files PLUS:** ConfigPage, ExtrasPage, CrashLogsPage, FFmpegLogsPage, CropWindow, FFmpegLogWindow, LicensesWindow, OverlayWindow, PreviewWindow, WebCamWindow, WebcamControl

---

## Safe to Modify in BOTH Branches

### ✅ ALWAYS SAFE (Backend):
```
src/OsrStudio.Core/                    - Core logic
src/OsrStudio.Audio/                   - Audio handling
src/OsrStudio.FFmpeg/                  - FFmpeg integration
src/OsrStudio.Windows/                 - Windows backend
src/Screna/                            - Screen capture
src/OsrStudio.Hotkeys/                 - Hotkeys
src/OsrStudio.Imgur/                   - Imgur upload
```

### ⚠️ CHECK FIRST (ViewModels):
```
src/OsrStudio.ViewCore/                - May have UI-specific refs
```

### 🚫 NEVER Cherry-Pick (UI):
```
src/OsrStudio/Pages/                   - Different per branch
src/OsrStudio/Windows/                 - Different per branch
src/OsrStudio/Controls/                - Different per branch
```

---

## Workflows

### Backend Change (FFmpeg, Core, etc.)
```bash
# Make change on one branch
git checkout main
# Edit backend file
git commit -m "Fix: Description"
git push origin main

# Apply to other branch
git checkout classic-ui
git cherry-pick main
git push origin classic-ui
```

### UI Change (Pages, Windows, Controls)
```bash
# Check if files differ between branches
git diff main:src/OsrStudio/Pages/WebcamPage.xaml \
         classic-ui:src/OsrStudio/Pages/WebcamPage.xaml

# If different → Apply changes SEPARATELY on each branch
# DO NOT cherry-pick
```

---

## Quick Branch Check

```bash
# Which branch?
git branch --show-current

# Verify by checking for classic UI marker
ls src/OsrStudio/Pages/ | grep ConfigPage
# Found → classic-ui
# Not found → main
```

---

## Recovery

### Reset to Clean State
```bash
# Find good commit
git reflog

# Reset branch
git reset --hard <commit>
git push origin <branch> --force
```

---

## Rules

1. **Always verify branch:** `git branch --show-current`
2. **Backend changes → Usually safe to cherry-pick**
3. **UI changes (.xaml, Pages/, Windows/) → Check first, often need manual application**
4. **Test builds after every change**

---

## Emergency Commands

```bash
# See all differences
git diff main classic-ui --stat

# Files only on classic-ui
git diff main classic-ui --name-status | grep "^A"

# Files only on main
git diff classic-ui main --name-status | grep "^A"
```
