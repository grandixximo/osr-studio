# Hotkeys Guide

OSR Studio supports **customizable keyboard shortcuts** (hotkeys) for quick access to recording and screenshot functions. This guide covers the default hotkeys and how to configure your own.

## 🎮 Default Hotkeys

OSR Studio comes with the following default hotkeys configured:

| Action | Default Hotkey | Description |
|--------|----------------|-------------|
| **Start/Stop Recording** | `Alt` + `F9` | Start a new recording or stop the current recording |
| **Pause/Resume Recording** | `Shift` + `F9` | Pause the current recording or resume a paused recording |
| **Screenshot** | `Print Screen` | Take a screenshot of the selected region/screen |
| **Active Window Screenshot** | `Alt` + `Print Screen` | Capture the currently active window |
| **Desktop Screenshot** | `Shift` + `Print Screen` | Capture the entire desktop |

## 📋 Available Hotkey Actions

You can assign hotkeys to the following actions:

### Recording Actions
- **Start/Stop Recording**: Toggle recording on/off
- **Pause/Resume Recording**: Pause/resume the current recording

### Screenshot Actions
- **Screenshot**: Take a screenshot based on your current video source selection
- **Desktop Screenshot**: Capture all screens (full desktop)
- **Active Window Screenshot**: Capture the currently focused window
- **Screenshot Region**: Capture using the region selector
- **Screenshot Screen**: Capture using the screen picker
- **Screenshot Window**: Capture using the window picker

### Overlay Actions
- **Toggle Mouse Clicks**: Show/hide mouse click overlay
- **Toggle Keystrokes**: Show/hide keystroke overlay

### Window Actions
- **Show Main Window**: Bring the main OSR Studio window to front
- **Toggle Region Picker**: Open or close the region selection tool

## ⚙️ Configuring Hotkeys

### Using the Settings UI

1. **Open OSR Studio**
2. **Go to Settings** (click the gear icon or press the hotkey if configured)
3. **Navigate to Hotkeys tab**
4. **Configure your hotkeys:**
   - Select a service/action from the dropdown
   - Click in the hotkey field
   - Press your desired key combination
   - Check the "Active" checkbox to enable the hotkey
5. **Save** - Hotkeys are saved automatically

### Adding a New Hotkey

To add a completely new hotkey:

1. Click the **"+" (Add)** button in the Hotkeys settings
2. Select the **Service** (action) you want to trigger
3. Click in the **Key** field and press your desired combination
4. Enable it by checking **"Active"**

### Removing a Hotkey

To remove a hotkey you no longer need:

1. Find the hotkey in the list
2. Click the **"Remove"** or **"Delete"** button next to it

### Resetting to Defaults

If you want to restore the default hotkeys:

1. Look for the **"Reset"** or **"Restore Defaults"** button in the Hotkeys settings
2. This will remove all custom hotkeys and restore the defaults

## 🔧 Technical Details

### How Hotkeys Work

OSR Studio's hotkey system is implemented using Windows' global hotkey registration:

1. **Global Registration**: Hotkeys are registered globally with Windows
   - Works even when OSR Studio is minimized or in the background
   - Uses `RegisterHotKey` Windows API

2. **Key Storage**: Hotkeys are saved to a configuration file
   - Location: `%AppData%/OsrStudio/Hotkeys.json` (or your settings directory)
   - Format: JSON with key codes and modifiers

3. **Conflict Detection**: Windows will prevent registration if another app uses the same hotkey
   - If a hotkey fails to register, it means another application is using it
   - Try a different combination

### Hotkey Configuration File

Hotkeys are stored in `Hotkeys.json` with this structure:

```json
[
  {
    "ServiceName": "Recording",
    "Key": "F9",
    "Modifiers": "Alt",
    "IsActive": true
  },
  {
    "ServiceName": "ScreenShot",
    "Key": "PrintScreen",
    "Modifiers": "None",
    "IsActive": true
  }
]
```

**Fields:**
- `ServiceName`: The action to trigger (see [Available Actions](#available-hotkey-actions))
- `Key`: The main key (function key, letter, Print Screen, etc.)
- `Modifiers`: Modifier keys (None, Alt, Ctrl, Shift, or combinations like "Ctrl+Alt")
- `IsActive`: Whether the hotkey is enabled

### Supported Modifiers

You can combine keys with these modifiers:

- **None**: Just the key itself (e.g., `Print Screen`)
- **Alt**: Alt key
- **Ctrl**: Control key  
- **Shift**: Shift key
- **Win**: Windows key (use with caution - may conflict with system hotkeys)

**Combinations** (can be combined):
- `Ctrl` + `Alt`
- `Ctrl` + `Shift`
- `Alt` + `Shift`
- `Ctrl` + `Alt` + `Shift`

### Supported Keys

Almost any key can be used, including:

- **Function Keys**: F1-F24
- **Letters**: A-Z
- **Numbers**: 0-9 (both top row and numpad)
- **Special Keys**: Print Screen, Scroll Lock, Pause
- **Numpad Keys**: Multiply, Add, Subtract, Divide, etc.
- **Media Keys**: Play, Stop, Next, Previous, Volume Up/Down

## 💡 Hotkey Best Practices

### Choosing Good Hotkeys

1. **Avoid System Shortcuts**: Don't conflict with Windows shortcuts
   - Avoid `Win` + key combinations (reserved by Windows)
   - Be careful with `Alt` + `F4`, `Ctrl` + `C`, etc.

2. **Use Modifiers**: Always use at least one modifier (Alt, Ctrl, Shift)
   - Prevents accidental triggers
   - Reduces conflicts with other apps
   - Exception: Print Screen is commonly used alone

3. **Be Consistent**: Use a logical pattern
   - Example: All recording actions use `F9` with different modifiers
   - Screenshots use `Print Screen` with modifiers

4. **Consider Your Workflow**: Think about what's comfortable
   - Use keys near your left hand if you need quick access
   - Function keys (F1-F12) are good for frequently used actions

5. **Test for Conflicts**: After setting a hotkey, test it
   - If it doesn't work, another app might be using it
   - Try a different combination

### Common Hotkey Patterns

Here are some suggested patterns:

**Recording-focused:**
- `F9` alone: Start/Stop Recording
- `Shift` + `F9`: Pause/Resume
- `Ctrl` + `F9`: Toggle mouse clicks

**Screenshot-focused:**
- `Print Screen`: Current selection
- `Alt` + `Print Screen`: Active window
- `Ctrl` + `Print Screen`: Region picker
- `Shift` + `Print Screen`: Full desktop

## 🐛 Troubleshooting

### My hotkey doesn't work

**Possible causes:**

1. **Not Active**: Make sure the checkbox is checked in settings
2. **Conflict**: Another application is using the same hotkey
   - Try a different key combination
   - Check apps like Discord, OBS, or screenshot tools

3. **Invalid Key**: Some key combinations can't be registered
   - Windows reserves some combinations
   - Try adding/changing modifiers

4. **Permission Issues**: Some keys require administrator privileges
   - Try running OSR Studio as administrator

### How do I find what's using my hotkey?

Unfortunately, Windows doesn't provide a built-in way to see this. Try:

1. Close other likely applications (Discord, OBS, screenshot tools)
2. Try registering the hotkey again
3. Use trial and error to identify the conflicting app

### Hotkeys not saving

Check that:
- OSR Studio has write permission to its settings directory
- The settings directory exists: `%AppData%/OsrStudio/`
- The `Hotkeys.json` file isn't read-only

### Restore deleted Hotkeys.json

If you accidentally delete the configuration file:

1. Close OSR Studio
2. Delete `%AppData%/OsrStudio/Hotkeys.json` (if it exists)
3. Restart OSR Studio
4. Use the "Reset" button in Hotkeys settings to restore defaults

## 🔍 Advanced: Manual Configuration

### Editing Hotkeys.json Directly

For advanced users, you can edit the hotkeys file directly:

1. **Close OSR Studio** (important!)
2. **Navigate to**: `%AppData%/OsrStudio/`
3. **Open**: `Hotkeys.json` in a text editor
4. **Edit** the JSON following the format above
5. **Save** and restart OSR Studio

**Available ServiceName values:**
- `Recording`
- `Pause`
- `ScreenShot`
- `DesktopScreenShot`
- `ActiveScreenShot`
- `ScreenShotRegion`
- `ScreenShotScreen`
- `ScreenShotWindow`
- `ToggleMouseClicks`
- `ToggleKeystrokes`
- `ShowMainWindow`
- `ToggleRegionPicker`

**Modifier values:**
- `None` = 0
- `Alt` = 1
- `Ctrl` = 2
- `Shift` = 4
- `Win` = 8

Combine modifiers by adding values (e.g., `Ctrl` + `Alt` = 3, `Ctrl` + `Shift` = 6)

**Key values:** Use .NET's `Keys` enum values:
- Letters: `A`, `B`, `C`, etc.
- Numbers: `D0`-`D9` (top row) or `NumPad0`-`NumPad9`
- Function keys: `F1`, `F2`, etc.
- Special: `PrintScreen`, `Scroll`, `Pause`
- Full list: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys

**Example:**

```json
[
  {
    "ServiceName": "Recording",
    "Key": "R",
    "Modifiers": 3,
    "IsActive": true
  }
]
```
This creates `Ctrl` + `Alt` + `R` to start/stop recording.

## 📝 Notes

- **System-wide**: Hotkeys work globally, even when OSR Studio is minimized
- **Instant Effect**: Most hotkey changes take effect immediately
- **Backup**: Consider backing up `Hotkeys.json` if you create a complex setup
- **Portable Mode**: In portable mode, `Hotkeys.json` is stored in the app folder

## 🙏 Credits

Hotkey system designed and implemented by **Mathew Sachin** (original Captura creator).

The system uses Windows' native global hotkey registration for reliable, system-wide shortcuts.

---

**Need more help?** Check the [FAQ](FAQ.md) or [open an issue](https://github.com/grandixximo/osr-studio/issues) on GitHub.

