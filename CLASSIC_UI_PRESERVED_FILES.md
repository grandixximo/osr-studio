# Classic UI Preserved Files

## Backup Branch
`classic-ui-backup-before-rebranding` - Created before rebranding

## Critical Classic UI Files (DO NOT MERGE FROM MAIN)

### Main UI Structure
- `src/Captura/Pages/MainPage.xaml` - Full classic toolbar and controls
- `src/Captura/Pages/MainPage.xaml.cs` - Classic page logic
- `src/Captura/Windows/MainWindow.xaml` - Classic window chrome
- `src/Captura/Windows/MainWindow.xaml.cs` - Classic window logic

### Classic-Specific Controls
- `src/Captura/Controls/*.xaml` - All custom controls
- `src/Captura/Controls/*.xaml.cs` - All control code-behind

### Classic-Specific Pages
- All pages in `src/Captura/Pages/` that differ from modern UI

### Build Configuration
- `CLASSIC_UI_BUILD.md` - Classic-specific build notes

## Rebranding Strategy
- Rename folders: Captura.* → OsrStudio.*
- Update namespaces: Captura → OsrStudio
- Preserve UI structure and layout
- Keep all classic functionality intact
