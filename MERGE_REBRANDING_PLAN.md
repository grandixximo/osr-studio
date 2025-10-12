# Merge Rebranding Plan: Main → Classic UI Branch

## Executive Summary

This document outlines the strategy to merge the rebranding changes from `main` branch (which uses `OsrStudio.*` naming) into the `cursor/merge-rebranding-into-classic-ui-5085` branch while preserving the classic UI style and functionality.

## Key Differences Identified

### 1. Naming Convention Changes (Main Branch Rebranding)
- **Folders**: `Captura.*` → `OsrStudio.*`
- **Namespaces**: `Captura` → `OsrStudio`
- **Project Files**: All `.csproj` files use new names
- **Solution File**: References updated project names

### 2. UI Architecture Differences

#### Classic UI Branch (Current)
- **MainPage.xaml**: Contains full UI with toolbars, buttons, video source controls
- **Complex Layout**: All controls visible and accessible from main page
- **Traditional Windows App**: Desktop-style interface with toolbar buttons
- **Direct Controls**: Settings, refresh, folder buttons in header

#### Modern UI Branch (Main)
- **MainPage.xaml**: Simplified - just loads `HomePage.xaml` in a Frame
- **Minimal Layout**: Navigation-based structure
- **Modern Design**: Page-based navigation pattern

### 3. Files That MUST Be Preserved from Classic UI

#### Critical UI Files (Classic-Specific)
```
src/Captura/Pages/MainPage.xaml
src/Captura/Pages/MainPage.xaml.cs
src/Captura/Pages/HomePage.xaml (if exists)
src/Captura/Pages/HomePage.xaml.cs (if exists)
src/Captura/Windows/MainWindow.xaml
src/Captura/Windows/MainWindow.xaml.cs
src/Captura/Controls/*.xaml (all control files)
src/Captura/Controls/*.xaml.cs (all control code-behind)
```

#### Classic-Specific Features
- Toolbar layout
- Button positioning
- Status bar implementation
- Video source selection UI
- Recording control layout

## Merge Strategy

### Phase 1: Preparation & Analysis ✅
1. ✅ Analyzed main branch rebranding (folder names, namespaces)
2. ✅ Analyzed classic UI branch structure
3. ✅ Identified UI differences (MainPage complexity)
4. ✅ Documented critical files to preserve

### Phase 2: Pre-Merge Backup
1. Create backup branch: `classic-ui-backup`
2. Document all classic UI-specific XAML files
3. Create list of all UI layout differences
4. Save original MainPage.xaml, MainWindow.xaml, and key Controls

### Phase 3: Systematic Rebranding (Manual Approach)

**Why Manual Instead of Git Merge?**
- The branches have no common merge base
- Main branch has completely different UI structure
- Need surgical precision to preserve classic UI while applying rebranding
- Avoid merge conflicts that would overwrite classic UI

**Approach: Rename in place, then pull functional updates**

#### Step 3.1: Folder Renaming
```bash
cd src/
mv Captura OsrStudio
mv Captura.Audio OsrStudio.Audio
mv Captura.Base OsrStudio.Base
mv Captura.Console OsrStudio.Console
mv Captura.Core OsrStudio.Core
mv Captura.Fakes OsrStudio.Fakes
mv Captura.FFmpeg OsrStudio.FFmpeg
mv Captura.Hotkeys OsrStudio.Hotkeys
mv Captura.Loc OsrStudio.Loc
mv Captura.MouseKeyHook OsrStudio.MouseKeyHook
mv Captura.NAudio OsrStudio.NAudio
mv Captura.SharpAvi OsrStudio.SharpAvi
mv Captura.ViewCore OsrStudio.ViewCore
mv Captura.Windows OsrStudio.Windows
mv Captura.YouTube OsrStudio.YouTube
mv Captura.sln OsrStudio.sln
```

#### Step 3.2: Project File Updates
Update all `.csproj` files:
- Change `<RootNamespace>Captura*</RootNamespace>` → `<RootNamespace>OsrStudio*</RootNamespace>`
- Change `<AssemblyName>Captura*</AssemblyName>` → `<AssemblyName>OsrStudio*</AssemblyName>`
- Update `<ProjectReference>` paths to new folder names

#### Step 3.3: Solution File Update
Update `OsrStudio.sln`:
- Change all project references from `Captura.*` to `OsrStudio.*`
- Update project GUIDs if necessary
- Update folder paths

#### Step 3.4: Namespace Replacement in C# Files
**Automated approach using find/replace:**
```bash
# Find all C# files and replace namespace declarations
find src/ -name "*.cs" -type f -exec sed -i 's/namespace Captura\b/namespace OsrStudio/g' {} +
find src/ -name "*.cs" -type f -exec sed -i 's/using Captura\b/using OsrStudio/g' {} +
```

**Manual verification needed for:**
- Comments mentioning "Captura"
- String literals with "Captura"
- Documentation URLs

#### Step 3.5: XAML File Updates
**Update namespace declarations:**
```bash
# Update xmlns declarations in XAML files
find src/ -name "*.xaml" -type f -exec sed -i 's/clr-namespace:Captura\b/clr-namespace:OsrStudio/g' {} +
find src/ -name "*.xaml" -type f -exec sed -i 's/x:Class="Captura\./x:Class="OsrStudio./g' {} +
```

**Manual verification for:**
- Resource references
- Style references
- Custom control declarations

#### Step 3.6: App.xaml and App.config Updates
- Update startup assembly references
- Update configuration namespace references
- Update resource dictionary references

### Phase 4: Preserve Classic UI Layout

**Critical: After renaming, verify these files retain classic UI structure:**

1. **MainPage.xaml** - Must keep full toolbar with:
   - Refresh, Open Output Folder, Settings buttons
   - Cursor, Clicks, Keystrokes toggle buttons
   - Video source selection controls
   - Recording controls

2. **MainWindow.xaml** - Must keep:
   - Classic window chrome
   - Title bar style
   - Window sizing and layout

3. **All Controls** - Preserve all custom controls in Controls/ folder

### Phase 5: Documentation Updates

Update all documentation files:
```
README.md - Update with OsrStudio branding but note "Classic UI"
docs/*.md - Update references
CLASSIC_UI_BUILD.md - Update build instructions
```

### Phase 6: Build Verification

1. Restore NuGet packages
2. Build solution
3. Fix any remaining namespace issues
4. Test key functionality:
   - Window opens correctly
   - Toolbar buttons work
   - Recording functions work
   - Classic UI layout preserved

### Phase 7: Final Merge of Non-UI Files

After rebranding is complete and builds successfully:
```bash
# Selectively merge non-UI improvements from main
git checkout main -- .github/
git checkout main -- docs/ (verify doesn't break classic UI docs)
git checkout main -- licenses/
```

## Risk Mitigation

### High Risk Areas
1. **MainPage.xaml** - Complete UI structure difference
   - **Mitigation**: Never merge this file, only rename namespaces

2. **Project References** - Circular dependencies
   - **Mitigation**: Update .csproj files systematically

3. **Resource Dictionaries** - Style references
   - **Mitigation**: Test UI rendering after each step

### Rollback Plan
- Backup branch created before starting
- Git reflog available for recovery
- Step-by-step approach allows incremental rollback

## Success Criteria

- ✅ All folders renamed to OsrStudio.*
- ✅ All namespaces use OsrStudio
- ✅ Solution builds without errors
- ✅ Classic UI layout fully preserved
- ✅ MainPage.xaml retains all toolbar buttons and controls
- ✅ Application launches and functions correctly
- ✅ Documentation updated with rebranding
- ✅ Build scripts updated for "OSR Studio Classic"

## Timeline Estimate

- Phase 1: ✅ Complete (Analysis)
- Phase 2: 15 minutes (Backup)
- Phase 3: 2-3 hours (Systematic rebranding)
- Phase 4: 1 hour (Verify UI preservation)
- Phase 5: 30 minutes (Documentation)
- Phase 6: 1 hour (Build and test)
- Phase 7: 30 minutes (Selective merges)

**Total: ~5-6 hours of careful, systematic work**

## Notes

- This is a **rename and rebrand** operation, not a traditional git merge
- The goal is to apply naming changes while keeping classic UI intact
- Main branch UI structure is incompatible with classic UI requirements
- Manual approach gives precise control over what changes are applied
- Classic UI must remain fully functional throughout

## Next Steps

1. Get approval for this plan
2. Create backup branch
3. Begin Phase 3 (Folder renaming)
4. Proceed systematically through each phase
5. Test thoroughly after each major step
