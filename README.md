<div align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://grandixximo.github.io/osr-studio/logo-dark.svg">
    <img src="https://grandixximo.github.io/osr-studio/logo-light.svg" alt="OSR Studio" width="300">
  </picture>
  
  <h3>Open Screen Recorder - Powerful Screen Recording Made Simple</h3>
  <p>Capture screen, webcam, audio, cursor, mouse clicks and keystrokes</p>

  [![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE.md)
  ![Build Status](https://github.com/grandixximo/osr-studio/actions/workflows/build.yml/badge.svg)
  
  [Website](https://grandixximo.github.io/osr-studio/) • [Download](https://github.com/grandixximo/osr-studio/releases/latest) • [Documentation](#docs)
</div>



&copy; [Copyright 2019](mathew/LICENSE_MathewSachin.md) Mathew Sachin  
&copy; [Copyright 2024](LICENSE.md) Mr. Chip  
&copy; [Copyright 2025](LICENSE.md) grandixximo

To read the story of **Capture** in following
:link: <https://mathewsachin.github.io/Captura/>

---

<div align="center">

### 🎬 Modern & Classic UI

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://grandixximo.github.io/osr-studio/modern-ui-dark.png">
  <img src="https://grandixximo.github.io/osr-studio/modern-ui-light.png" alt="OSR Studio Modern UI" width="45%">
</picture>
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://grandixximo.github.io/osr-studio/classic-ui-dark.png">
  <img src="https://grandixximo.github.io/osr-studio/classic-ui-light.png" alt="OSR Studio Classic UI" width="45%">
</picture>

</div>


## About This Project

**OSR Studio (Open Screen Recorder)** is a complete rebrand of the excellent screen recording software originally known as Captura. This project builds upon the foundation laid by its predecessors while establishing a fresh identity for better visibility and continued development.

### Origins & Credits

The original [Captura](https://github.com/MathewSachin/Captura) was created by **Mathew Sachin** - an exceptional tool that was small, portable, and just worked perfectly. When the original project was discontinued, [Mr. Chip (mrchipset)](https://github.com/mrchipset/nCaptura) maintained an excellent fork (nCaptura) that kept the project alive and functional.

Open Screen Recorder (OSR Studio) continues this legacy with important enhancements:

- ✅ Fixed the FFmpeg download window initialization issue
- ✅ Updated FFmpeg download mirrors to ensure reliability
- ✅ Improved build and release workflows
- ✅ Restored and fixed the classic 8.0.0 UI (dual-release with modern UI)
- ✅ Fixed numerous data binding issues, UI bugs, and preview windows
- ✅ Added shake animations, live region updates, and improved UX
- ✅ Added AMD AMF hardware encoding support (inspired by OBS Studio)
- ✅ Implemented Windows Graphics Capture (WGC) for reliable screen recording (Windows 10 1903+)
- ✅ Complete rebrand for better project visibility

**Huge thanks to:**
- **Mathew Sachin** - Original creator of Captura, the foundation of this project
- **Mr. Chip (mrchipset)** - Excellent maintainer who kept the project alive with nCaptura
- **Cursor & Claude** - AI tools that made these enhancements possible


## ✨ Features

<table>
<tr>
<td width="50%">

### 🎥 Recording
- **Screen Recording** - Capture in AVI, GIF, MP4
- **WebCam Capture** - Record from camera
- **Hardware Encoding** - AMD AMF support
- **Windows Graphics Capture** - Modern WGC support (Win 10 1903+)

</td>
<td width="50%">

### 🎨 Capture Options
- **Screenshots** - Capture regions, screens, or windows
- **Mouse Cursor** - Include/exclude cursor
- **Click & Keystroke** - Record mouse clicks and keystrokes
- **Multi-Audio** - Mix microphone and speaker output

</td>
</tr>
<tr>
<td width="50%">

### ⚙️ Advanced
- **Command-line** - [CLI support](docs/Cmdline/README.md) (*BETA*)
- **Hotkeys** - [Configurable shortcuts](docs/hotkeys.md)

</td>
<td width="50%">

### 🌍 Accessibility  
- **Multi-language** - [Available in multiple languages](docs/translation.md)
- **Portable** - No installation required option

</td>
</tr>
</table>

## 📦 Installation

[latest]: https://github.com/grandixximo/osr-studio/releases/latest

Get the latest release from **[GitHub Releases][latest]**

### Download Options

| Type | File | Description |
|------|------|-------------|
| 🏠 **Installer** | `OSR-Studio-Setup.exe` or `OSR-Studio-Classic-Setup.exe` | Recommended for most users - Includes automatic updates |
| 📦 **Portable** | `OSR-Studio-Portable.zip` or `OSR-Studio-Classic-Portable.zip` | No installation required - Run from anywhere |

### 🔨 Build from Source

See the [Build Notes](docs/Build.md) for detailed instructions on building from source.

## 📚 Documentation

<details>
<summary><b>📖 Quick Links</b></summary>

### Getting Started
- [System Requirements](docs/System-Requirements.md)
- [FAQ](docs/FAQ.md)
- [ScreenShots](docs/Screenshots)

### Configuration
- [Command-line Usage](docs/Cmdline/README.md)
- [Hotkeys](docs/hotkeys.md)
- [FFmpeg Setup](docs/FFmpeg.md)

### Development
- [Build Notes](docs/Build.md)
- [Contributing](CONTRIBUTING.md)
- [GitHub Actions Workflows](.github/workflows/README.md)
- [Code of Conduct](CODE_OF_CONDUCT.md)
- [Changelog](docs/Changelogs/README.md)

</details>

## License

[MIT License](LICENSE.md)

Check [here](licenses/) for licenses of dependencies.
