# Building

## Setting up locally

### Prerequisites
- Visual Studio 2022 or newer with .NET desktop development workload
- .NET Core 2.1 or greater
- Some features have other specific requirements, see [System Requirements](System-Requirements.md)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/grandixximo/osr-studio.git
   cd osr-studio
   ```

2. **Setup API Keys** (Optional - only needed for specific features)
   
   These are loaded from environment variables during development and embedded into the app on production builds.
   
   | Environment Variable | Description | Required For |
   |---------------------|-------------|--------------|
   | `imgur_client_id` | Imgur Client Id | Imgur uploads |
   | `yt_client_id` | YouTube Client Id | YouTube uploads |
   | `yt_client_secret` | YouTube Client Secret | YouTube uploads |

   **Note:** YouTube credentials are only required if you want to upload to YouTube. See [YouTube API documentation](https://developers.google.com/youtube/registering_an_application) for more info.

3. **Download FFmpeg**
   - Download from within the app, or
   - Get it from https://www.gyan.dev/ffmpeg/builds/, or
   - Use a custom build

4. **Build the solution**
   
   **Option A: Using Visual Studio**
   - Open `src/Captura.sln` in Visual Studio
   - Build → Rebuild Solution (or press `Ctrl+Shift+B`)
   - Run the project

   **Option B: Using MSBuild (Command Line)**
   ```bash
   # Restore NuGet packages
   nuget restore src/Captura.sln
   
   # Build the solution
   msbuild src/Captura.sln /p:Configuration=Release /p:Platform="Any CPU"
   ```

## Building Release Packages

Release packages are built automatically via **GitHub Actions** when you push a version tag.

### Creating a Release

1. **Tag a version**
   ```bash
   git tag v10.3.0
   git push origin v10.3.0
   ```

2. **GitHub Actions automatically:**
   - Builds both Modern and Classic UI versions
   - Creates installer packages (`.exe`)
   - Creates portable packages (`.zip`)
   - Publishes a GitHub Release with all artifacts

See `.github/workflows/dual-release.yml` for the complete build process.

### Manual Package Building

If you need to build packages locally:

**Portable Version:**
```bash
# Build in Release mode
msbuild src/Captura.sln /p:Configuration=Release

# Create dist folder and copy files
New-Item -ItemType Directory -Force -Path dist
Copy-Item -Path "src/Captura/bin/Release/*" -Destination "dist/" -Recurse

# Create portable markers
New-Item -ItemType Directory -Force -Path "dist/Settings"
New-Item -ItemType Directory -Force -Path "dist/Codecs"

# Zip it up
Compress-Archive -Path "dist/*" -DestinationPath "OSR-Studio-Portable.zip"
```

**Installer:**
1. Install [Inno Setup](https://jrsoftware.org/isinfo.php)
2. Build the solution in Release mode
3. Run Inno Setup on `Inno.iss`

## CI/CD

The project uses **GitHub Actions** for continuous integration and deployment:

- **Debug Builds** (`.github/workflows/debug-builds.yml`) - Manual workflow for testing
- **Dual Release** (`.github/workflows/dual-release.yml`) - Automatic release on version tags

See `.github/workflows/README.md` for more information about the CI/CD pipeline.