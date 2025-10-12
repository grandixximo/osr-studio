# Translation Guide

OSR Studio is available in **35 languages**, making it accessible to users worldwide. This guide explains how the translation system works and how you can contribute.

## 🌍 Available Languages

OSR Studio currently supports the following languages:

| Language | Code | Language | Code |
|----------|------|----------|------|
| Arabic | ar | Italian | it |
| Belarusian | be | Japanese | ja |
| Catalan | ca | Kabyle | kab |
| Czech | cs | Korean | ko |
| Danish | da | Malayalam | ml |
| German | de | Dutch | nl |
| Greek | el | Norwegian | no |
| English | en | Polish | pl |
| Spanish | es | Portuguese | pt |
| Finnish | fi | Portuguese (Brazil) | pt-BR |
| French | fr | Romanian | ro |
| Hebrew | he | Russian | ru |
| Hindi | hi | Slovenian | sl |
| Indonesian | id | Swedish | sv |
| Icelandic | is | Thai | th |
| | | Turkish | tr |
| | | Ukrainian | uk |
| | | Vietnamese | vi |
| | | Chinese (Simplified) | zh-CN |
| | | Chinese (Traditional) | zh-TW |

## 🔧 How Translation Works

### Technical Overview

OSR Studio uses a **JSON-based localization system**:

1. **Language Files**: Each language has a JSON file in `src/Captura.Loc/Languages/`
2. **Default Language**: English (`en.json`) serves as the base/fallback language
3. **Format**: Simple key-value pairs in JSON format
4. **Loading**: The `LanguageManager` class loads translations at runtime
5. **Fallback**: If a translation is missing, it falls back to English

### File Structure

Translation files follow this simple structure:

```json
{
  "About": "About",
  "AccentColor": "Accent Color",
  "Add": "Add",
  "AlwaysOnTop": "Always on Top",
  "Audio": "Audio",
  ...
}
```

**Example** (Spanish - `es.json`):

```json
{
  "About": "Acerca de",
  "AccentColor": "Color de tono",
  "Add": "Añadir",
  "AlwaysOnTop": "Siempre visible",
  "Audio": "Audio",
  ...
}
```

### How It Works in Code

The translation system is implemented in the `Captura.Loc` namespace:

- **`LanguageManager`**: Singleton that manages all languages
  - Scans the `Languages/` folder for `.json` files
  - Uses culture names (e.g., `en`, `es`, `zh-CN`) as filenames
  - Automatically detects available cultures
  - Provides fallback to English if a key is missing

- **`LanguageFields`**: Base class with properties for all translatable strings
  - Each UI string is a property
  - Uses `CallerMemberName` to match property names to JSON keys

- **Runtime Switching**: Users can change languages without restarting
  - Triggers `LanguageChanged` event
  - All UI elements update automatically via data binding

## 🤝 Contributing Translations

There are **two ways** to contribute translations to OSR Studio:

### Option 1: Crowdin (Recommended by Original Project)

The original Captura project used [Crowdin](https://crowdin.com/) for managing translations. Crowdin provides:

- ✅ Professional translation management interface
- ✅ Translation memory and suggestions
- ✅ Context for translators
- ✅ Quality checks and validation
- ✅ Collaboration tools

**However**, OSR Studio does not currently have an active Crowdin project set up. The `crowdin.yml` configuration file exists but needs to be activated by the project maintainers.

**Why Crowdin?** The original creator (Mathew Sachin) preferred Crowdin because:
- Centralized translation management
- Prevents duplicate/conflicting PRs
- Better quality control
- Easier for non-technical translators
- Automatic synchronization with the repository

### Option 2: Direct Pull Requests (Currently Available)

Since Crowdin is not currently active for OSR Studio, you can contribute translations via GitHub Pull Requests:

#### Step 1: Fork and Clone

```bash
# Fork the repository on GitHub, then clone your fork
git clone https://github.com/YOUR-USERNAME/osr-studio.git
cd osr-studio
```

#### Step 2: Choose Your Action

**A. Improve an Existing Translation**

```bash
# Open the language file you want to improve
# Example: Spanish
notepad src/Captura.Loc/Languages/es.json
```

**B. Add a New Language**

```bash
# Copy the English template
cp src/Captura.Loc/Languages/en.json src/Captura.Loc/Languages/YOUR-LANGUAGE-CODE.json

# Example: for Swedish
cp src/Captura.Loc/Languages/en.json src/Captura.Loc/Languages/sv.json
```

> **Language Code Format**: Use standard culture codes (ISO 639-1):
> - Two letters for most languages: `de`, `fr`, `ja`, `ko`
> - Extended codes for variants: `pt-BR`, `zh-CN`, `zh-TW`

#### Step 3: Edit the Translation File

Open the JSON file and translate the values (keep keys in English):

```json
{
  "About": "YOUR TRANSLATION",
  "AccentColor": "YOUR TRANSLATION",
  "Add": "YOUR TRANSLATION",
  ...
}
```

**Important Rules:**
- ✅ Keep all keys exactly as they are (in English)
- ✅ Only translate the values (text after the colon)
- ✅ Preserve special characters like `{0}`, `{1}` (placeholders)
- ✅ Keep the JSON format valid (commas, quotes, brackets)
- ✅ Use proper Unicode for non-Latin scripts
- ❌ Don't remove or rename keys
- ❌ Don't leave values empty (use English if unsure)

#### Step 4: Test Your Translation (Optional but Recommended)

If you can build the project:

```bash
# Build and run to see your translation in action
# See docs/Build.md for build instructions
```

The application will automatically detect your new language file if it's a valid culture code.

#### Step 5: Commit and Push

```bash
git add src/Captura.Loc/Languages/YOUR-LANGUAGE-CODE.json
git commit -m "Add/Update YOUR-LANGUAGE translation"
git push origin main
```

#### Step 6: Create a Pull Request

1. Go to your fork on GitHub
2. Click "Pull Request"
3. Provide a clear title: `Add Swedish translation` or `Update Spanish translation`
4. In the description, mention:
   - What language you're adding/updating
   - If you're a native speaker (helpful context)
   - Any notes about specific translations

### Translation Guidelines

When translating, consider:

1. **Context**: Some terms may have different meanings in different contexts
   - "Record" could mean "start recording" or "a recording"
   - Check where the string is used if possible

2. **Consistency**: Use the same translations for repeated terms
   - "ScreenShot" should always translate the same way

3. **UI Constraints**: Keep translations reasonably short
   - Very long translations might not fit in the UI
   - Test if possible, or note concerns in PR

4. **Technical Terms**: Some terms are commonly left in English
   - "FFmpeg", "Webcam", "Hotkey" - check what's standard in your language

5. **Placeholders**: Preserve format specifiers
   - `{0}` represents a value inserted at runtime
   - Example: `"Duration: {0} seconds"` → `"Duración: {0} segundos"`

## 🎯 Quick Reference for Translators

### Common Translation Keys

| Key | Context | Example (EN → ES) |
|-----|---------|-------------------|
| `Recording` | Start/stop recording | Recording → Grabación |
| `ScreenShot` | Take a screenshot | ScreenShot → Captura de pantalla |
| `Pause` | Pause recording | Pause → Pausar |
| `Resume` | Resume recording | Resume → Reanudar |
| `Settings` | Application settings | Settings → Configuración |
| `Exit` | Close application | Exit → Salir |

### Files to Translate

There is **only ONE file per language** to translate:
- `src/Captura.Loc/Languages/[language-code].json`

All UI strings are centralized in this single file.

## ❓ FAQ

### Why was the original creator (Mathew Sachin) hesitant about translation PRs?

The original Captura project used Crowdin for translations because:

1. **Coordination**: Multiple people might submit translations for the same language simultaneously, creating conflicts
2. **Quality Control**: Crowdin provides review and validation workflows
3. **Maintenance**: Direct PRs mean the maintainer has to manage translation updates manually
4. **Non-technical Contributors**: Crowdin is easier for translators who aren't familiar with Git

However, **OSR Studio welcomes translation PRs** since Crowdin is not currently set up. Just be aware that:
- Check if a translation PR already exists before starting
- Be prepared for review feedback
- The maintainers may eventually migrate to Crowdin

### Can I submit partial translations?

Yes! Partial translations are better than no translation. The system falls back to English for missing keys, so users will see:
- Your translations for completed strings
- English for strings you haven't translated yet

Just note in your PR that it's a partial translation.

### How do I know if my language code is correct?

Use standard **ISO 639-1** codes:
- Check: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
- For regional variants, use the format: `language-REGION` (e.g., `pt-BR`, `zh-CN`)
- The code must be a valid .NET `CultureInfo` name

### Will my translation be reviewed?

Yes, translation PRs will be reviewed by:
- The maintainers (for format and technical correctness)
- Ideally, native speakers in the community

Feedback is meant to improve quality, not to discourage contributions!

## 🙏 Credits

Translation system designed and implemented by **Mathew Sachin** (original Captura creator).

Thanks to [Crowdin](https://crowdin.com/) for providing open-source licenses for translation management.

Special thanks to all the community contributors who have helped translate OSR Studio into dozens of languages!

---

**Want to contribute?** Start by checking the [Contributing Guide](../CONTRIBUTING.md) and the existing [language files](../src/Captura.Loc/Languages/).

