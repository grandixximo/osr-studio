# Rebranding Checklist: nCaptura → OSR Studio

This checklist ensures complete rebranding when importing the nCaptura codebase.

## High Priority

### Code
- [ ] Replace all `nCaptura` namespace references with `OSRStudio`
- [ ] Update assembly names and project names
- [ ] Change application window titles
- [ ] Update assembly info files
- [ ] Rename solution file if needed
- [ ] Update internal application identifiers

### Documentation
- [x] Create README.md with new branding
- [x] Create CONTRIBUTORS.md crediting original developers
- [x] Create MIGRATION.md guide
- [x] Add .gitignore for project
- [ ] Update any existing documentation files
- [ ] Update code comments with old branding

### Visual Assets
- [ ] Replace application icon
- [ ] Update splash screen (if any)
- [ ] Change about dialog branding
- [ ] Update UI theme names if they reference nCaptura

### Configuration
- [ ] Update default config file names
- [ ] Change application data folder names
- [ ] Update registry keys (Windows)
- [ ] Modify installation paths

## Medium Priority

### Build & Distribution
- [ ] Update build scripts
- [ ] Modify installer configuration
- [ ] Change package names
- [ ] Update CI/CD pipeline references
- [ ] Modify release notes template

### External References
- [ ] Update any hardcoded URLs
- [ ] Change feedback/bug report URLs
- [ ] Modify update check endpoints (if any)
- [ ] Update analytics/telemetry identifiers (if any)

## Low Priority

### Cleanup
- [ ] Remove PayPal donation buttons/links
- [ ] Remove donation-related code
- [ ] Clean up any sponsor references
- [ ] Remove old branding assets

### Legal
- [ ] Update LICENSE file if needed (keep original license)
- [ ] Add attribution notices as required
- [ ] Update copyright notices (add new, keep original)

## Testing

After rebranding:
- [ ] Build succeeds on all configurations
- [ ] Application launches without errors
- [ ] Settings save/load correctly
- [ ] No visual artifacts of old branding
- [ ] All features work as expected

## Notes

- **Keep**: Original license terms and credits
- **Remove**: PayPal/donation references (per Matthew Sachin's wishes)
- **Maintain**: Both Classic and Modern UI branches
- **Preserve**: All functionality from nCaptura

## Search Patterns

When importing code, search for these patterns:

```bash
# Case-insensitive search for nCaptura
grep -ri "ncaptura" .

# Case-insensitive search for just Captura (be careful - may have many results)
grep -ri "captura" .

# Search for PayPal references
grep -ri "paypal" .

# Search for donation references
grep -ri "donat" .
```

Replace with OSR Studio equivalent terminology.
