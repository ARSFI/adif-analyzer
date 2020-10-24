; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ADIF Analyzer"
#define MyAppVersion "1.0.7.0"
#define MyAppPublisher "Amateur Radio Safety Foundation, inc."
#define MyAppURL "http://www.winlink.org"
#define MyAppExeName "ADIF Analyzer.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{16FD3AF3-49BD-477C-A2E8-1A1C15987C1E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=C:\RMS\ADIF Analyzer
DefaultGroupName={#MyAppName}
OutputDir=C:\PhilTFSSource\Winlink_Utilities\ADIF Analyzer\Installer
OutputBaseFilename=ADIF_Analyzer_install
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\PhilTFSSource\Winlink_Utilities\ADIF Analyzer\ADIF Analyzer\bin\Release\ADIF Analyzer.exe"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\ADIF Analyzer\bin\Release\Ionic.Zip.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\nsoftware.IPWorks.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\nsoftware.IPWorks.System.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\SyslogLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\WinlinkInterop.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\WinlinkRegistration.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ADIF Analyzer\bin\Release\WinlinkServiceClasses.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Support\ADIF Analyzer Revision History.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Support\CountryCodes.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Support\WorldMap.jpg"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "C:\PhilTFSSource\Winlink_Utilities\ADIF Analyzer\ADIF Analyzer\ADIF Analyzer.ico"
Name: "{commondesktop}\{#MyAppName}"; Filename: "C:\PhilTFSSource\Winlink_Utilities\ADIF Analyzer\ADIF Analyzer\ADIF Analyzer.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
