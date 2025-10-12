# GitHub Actions Optimization Plan

## Current State: 8 Workflow Files
- build.yml (OBSOLETE)
- build-dotnet-desktop-pull-request.yml (OBSOLETE)
- release-build-dotnet-desktop.yml (OBSOLETE)
- dual-release.yml (KEEP - Main release workflow)
- debug-builds.yml (KEEP - Manual testing)
- test-modern-ui.yml (REDUNDANT - triggers on every push)
- test-classic-ui.yml (BROKEN - branch doesn't exist)
- test-cursor-branches.yml (KEEP BUT MODIFY)

---

## Recommended: Keep 2-3 Workflows

### Minimal Setup (2 workflows):
1. **dual-release.yml** - Production releases on tags
2. **debug-builds.yml** - Manual testing (already perfect)

**Pros:**
- Minimal CI load (only runs on tags or manual trigger)
- Clear purpose for each workflow
- No redundant builds

**Cons:**
- No automatic PR validation
- Developers must manually test before merging

### Balanced Setup (3 workflows):
1. **dual-release.yml** - Production releases on tags
2. **debug-builds.yml** - Manual testing
3. **test-cursor-branches.yml** - Auto-test Cursor branches only

**Pros:**
- Still minimal CI load
- Automatic testing for Cursor branches
- Manual control for main development

**Cons:**
- Slightly more complex

---

## Specific Actions

### Delete These 5 Files:
```bash
rm .github/workflows/build.yml
rm .github/workflows/build-dotnet-desktop-pull-request.yml
rm .github/workflows/release-build-dotnet-desktop.yml
rm .github/workflows/test-modern-ui.yml
rm .github/workflows/test-classic-ui.yml
```

**Why:**
- `build.yml` - Replaced by test-modern-ui.yml and dual-release.yml
- `build-dotnet-desktop-pull-request.yml` - Old template, not used
- `release-build-dotnet-desktop.yml` - Replaced by dual-release.yml
- `test-modern-ui.yml` - Runs on EVERY push to main (excessive)
- `test-classic-ui.yml` - References non-existent branch

### Keep & Improve:
- **dual-release.yml** - Already optimal
- **debug-builds.yml** - Already optimal
- **test-cursor-branches.yml** - Already optimal (only runs on cursor/** branches)

---

## Additional Improvements

### 1. Fix dual-release.yml
**Issue:** References `classic-ui-modern-fixes` branch that doesn't exist

**Options:**
a) Create the branch
b) Remove classic UI build from dual-release
c) Make it conditional (skip if branch doesn't exist)

**Recommended:** Remove classic UI build since branch doesn't exist:

```yaml
# Remove the entire build-classic-ui job
# Remove create-release dependency on build-classic-ui
# Simplify to single UI release
```

### 2. Reduce Build Verbosity
Change all `verbosity:diag` to `verbosity:minimal` to reduce log spam

### 3. Add Concurrency Control
Add to workflows to cancel old runs:

```yaml
concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true
```

---

## Cost/Benefit Analysis

### Current State:
- 8 workflow files
- Triggers: Every push to main, PRs, cursor branches, and tags
- **Estimated runs per week:** 20-50+ (depending on activity)

### After Optimization:
- 3 workflow files (or 2 if ultra-minimal)
- Triggers: Tags (release), cursor branches only
- **Estimated runs per week:** 0-5 (mostly manual)

**Savings:** ~80-90% reduction in CI minutes

---

## Implementation Steps

1. **Backup current workflows** (already in git, so safe)
2. **Delete obsolete workflows** (5 files)
3. **Fix dual-release.yml** (remove or fix classic-ui build)
4. **Update README.md** (reflect new structure)
5. **Test the remaining workflows** (manual trigger)

---

## Final Structure

```
.github/workflows/
├── dual-release.yml          # Production: Triggers on tags
├── debug-builds.yml          # Manual: Full control testing
└── test-cursor-branches.yml  # Auto: Only cursor/** branches
```

**Total:** 3 workflows, minimal CI load, maximum control
