# How to Update Changelogs

This guide explains how to add new changelog entries for OSR Studio releases.

## Overview

The changelog system consists of:
- **JSON files** in `docs/Changelogs/` (stored in main and classic-ui branches)
- **Web page** at https://grandixximo.github.io/osr-studio/changelog.html (automatically loads from both branches)

## Adding a New Release

### 1. Edit the JSON File

Choose the appropriate file based on which branch you're releasing:
- `main.json` - For Modern UI (main branch)
- `classic-ui.json` - For Classic UI (classic-ui branch)

### 2. Add Release Entry

Add a new release object at the **beginning** of the `releases` array:

```json
{
  "version": "10.8.0",
  "date": "2025-11-15",
  "title": "Bug Fixes and Performance Improvements",
  "sections": [
    {
      "type": "added",
      "title": "New Features",
      "items": [
        "Added support for 4K recording",
        "New audio mixer interface"
      ]
    },
    {
      "type": "fixed",
      "title": "Bug Fixes",
      "items": [
        "Fixed crash when recording with external microphone",
        "Resolved memory leak in long recordings"
      ]
    }
  ]
}
```

### 3. Section Types

Use these section types to categorize changes:

| Type | Icon | Use For |
|------|------|---------|
| `added` | ✨ | New features |
| `changed` | 🔄 | Changes in existing functionality |
| `improved` | ⚡ | Enhancements and improvements |
| `fixed` | 🐛 | Bug fixes |
| `removed` | 🗑️ | Removed features |
| `deprecated` | ⚠️ | Soon-to-be removed features |
| `security` | 🔒 | Security fixes |

### 4. Date Format

- Use ISO format: `YYYY-MM-DD`
- For unreleased versions, use: `"2025-10-XX"` or leave "XX" for day
- The website will display "Coming Soon" for dates with "XX"

### 5. Commit and Push

```bash
# Add your changes
git add docs/Changelogs/main.json  # or classic-ui.json

# Commit
git commit -m "Add changelog for v10.8.0"

# Push to your branch
git push origin main  # or classic-ui
```

### 6. Website Updates Automatically

The changelog website fetches data directly from GitHub:
- **Main branch**: https://raw.githubusercontent.com/grandixximo/osr-studio/main/docs/Changelogs/main.json
- **Classic UI branch**: https://raw.githubusercontent.com/grandixximo/osr-studio/classic-ui/docs/Changelogs/classic-ui.json

Changes appear on the website within a few minutes after pushing to GitHub.

## Example: Complete Release Entry

Here's a comprehensive example:

```json
{
  "version": "10.7.0",
  "date": "2025-10-13",
  "title": "OSR Studio - Complete Rebrand",
  "sections": [
    {
      "type": "changed",
      "title": "Rebranding",
      "items": [
        "Complete rebrand from nCaptura to OSR Studio",
        "Updated application name, icons, and branding throughout",
        "New modern logo with light/dark theme variants"
      ]
    },
    {
      "type": "improved",
      "title": "Documentation",
      "items": [
        "Updated all documentation to reflect OSR Studio branding",
        "Enhanced README with better project description",
        "Added comprehensive credits section"
      ]
    },
    {
      "type": "fixed",
      "title": "Bug Fixes",
      "items": [
        "Fixed various UI inconsistencies after rebranding",
        "Corrected localization strings"
      ]
    }
  ]
}
```

## Tips

1. **Keep it concise** - Each item should be a single, clear statement
2. **Use imperative mood** - "Added support for X" not "Adds support for X"
3. **Be specific** - Include relevant details (versions, platforms, etc.)
4. **Group related changes** - Use sections to organize changes logically
5. **Test the JSON** - Make sure your JSON is valid before committing

## Viewing Changes

After pushing:
1. Visit https://grandixximo.github.io/osr-studio/changelog.html
2. Select the appropriate tab:
   - **Unified View** - See both branches together
   - **Modern UI** - Main branch only
   - **Classic UI** - Classic-ui branch only

## Troubleshooting

**Changes not showing on website?**
- Wait a few minutes for GitHub CDN to update
- Check that your JSON is valid (use a JSON validator)
- Verify you pushed to the correct branch
- Clear your browser cache (Ctrl+Shift+R or Cmd+Shift+R)

**JSON syntax error?**
- Use a JSON validator like https://jsonlint.com/
- Common issues: missing commas, trailing commas, unescaped quotes
- Make sure arrays and objects are properly closed

## Need Help?

If you encounter issues:
1. Check the browser console for JavaScript errors
2. Validate your JSON syntax
3. Review this guide carefully
4. Open an issue on GitHub if problems persist

