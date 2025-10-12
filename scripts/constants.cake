readonly var sourceFolder = Directory("src");
readonly var tempFolder = Directory("temp");
readonly var distFolder = Directory("dist");
readonly var licensesFolder = Directory("licenses");
readonly var chocoFolder = Directory("choco");

readonly var slnPath = sourceFolder + File("OsrStudio.sln");

readonly var PortablePath = tempFolder + File("OSR-Studio-Classic-Portable.zip");
readonly var SetupPath = tempFolder + File("OSR-Studio-Classic-Setup.exe");

const string Release = "Release";