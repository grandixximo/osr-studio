# Changelogs

OSR Studio maintains separate changelogs for each UI variant:

- **[Main Branch (Modern UI)](main.json)** - Modern UI changelog
- **[Classic UI Branch](classic-ui.json)** - Classic UI changelog

## View Online

Visit our [Changelog Website](https://grandixximo.github.io/osr-studio/changelog.html) to see a unified view of all changes.

## Format

Changelogs are stored in JSON format with the following structure:

```json
{
  "branch": "main",
  "title": "Branch Title",
  "description": "Description",
  "releases": [
    {
      "version": "10.7.0",
      "date": "2025-10-13",
      "title": "Release Title",
      "sections": [
        {
          "type": "added|changed|fixed|improved|removed|security",
          "title": "Section Title",
          "items": ["Change 1", "Change 2"]
        }
      ]
    }
  ]
}
```

## Adding New Entries

When preparing a new release:

1. Edit the appropriate JSON file (`main.json` or `classic-ui.json`)
2. Add a new release object to the `releases` array
3. Organize changes into sections by type:
   - **added** - New features
   - **changed** - Changes in existing functionality
   - **deprecated** - Soon-to-be removed features
   - **removed** - Removed features
   - **fixed** - Bug fixes
   - **security** - Security fixes
   - **improved** - Enhancements and improvements
4. Commit and push to your branch
5. The website will automatically display the updated changelog

## Legacy Changelogs

For historical reference, older markdown changelogs can be found in the git history:
- v9.0.0
- v8.0.0
- v7.0.1
