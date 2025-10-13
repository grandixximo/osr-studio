; Override version before compiling
;#define MyAppVersion "10.7.0"

; Override variant type: "Modern" or "Classic"
#ifndef Variant
  #define Variant "Modern"
#endif

#define MyAppName "OSR Studio"
#define MyAppPublisher "grandixximo"
#define MyAppURL "https://github.com/grandixximo/osr-studio"
#define MyAppExeName "OsrStudio.exe"

[Setup]
AppId={{C1670C5E-5042-4300-9491-6BFFF963823F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} v{#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\OSR-Studio
DisableProgramGroupPage=yes
ArchitecturesInstallIn64BitMode=x64compatible
OutputBaseFilename=OSR-Studio-Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=src/OsrStudio/Images/OsrStudio.ico
OutputDir=temp

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "ukrainian"; MessagesFile: "compiler:Languages\Ukrainian.isl"

[Registry]
; Store the variant type (Modern or Classic) so we can detect it later
Root: HKLM; Subkey: "SOFTWARE\OSR-Studio"; ValueType: string; ValueName: "UIVariant"; ValueData: "{#Variant}"; Flags: uninsdeletekey

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

; Remove Assemblies from previous installation to prevent conflicts
[InstallDelete]
Type: files; Name: "{app}\lib\*.dll"

[Files]
Source: "dist\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{commonprograms}\Open Screen Recorder"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\Open Screen Recorder"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
// Get the currently installed UI variant from registry
function GetInstalledVariant(): String;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\OSR-Studio', 'UIVariant', Result) then
    Result := '';
end;

// Check if another variant is already installed and block installation
function InitializeSetup(): Boolean;
var
  InstalledVariant: String;
  CurrentVariant: String;
  MsgText: String;
begin
  Result := True;
  CurrentVariant := '{#Variant}';
  InstalledVariant := GetInstalledVariant();
  
  // If a variant is installed and it's different from what we're trying to install
  if (InstalledVariant <> '') and (InstalledVariant <> CurrentVariant) then
  begin
    if InstalledVariant = 'Modern' then
      MsgText := 'OSR Studio (Modern UI) is currently installed.' + #13#10#13#10 +
                 'You cannot install OSR Studio Classic UI while the Modern UI version is installed.' + #13#10#13#10 +
                 'Please uninstall OSR Studio (Modern UI) first, then run this installer again.'
    else
      MsgText := 'OSR Studio (Classic UI) is currently installed.' + #13#10#13#10 +
                 'You cannot install OSR Studio Modern UI while the Classic UI version is installed.' + #13#10#13#10 +
                 'Please uninstall OSR Studio (Classic UI) first, then run this installer again.';
    
    MsgBox(MsgText, mbError, MB_OK);
    Result := False;
  end;
end;