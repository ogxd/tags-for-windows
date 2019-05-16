!define VERSION "1.0"

; The name of the installer
Name "Labels for Windows"

; The file to write
OutFile "LabelsForWindows-${VERSION}-win64.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\Labels for Windows"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "SOFTWARE\Labels for Windows" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

; The stuff to install
Section "Labels for Windows (required)"

  SectionIn RO

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  File "Tools\ComRegTool.exe"
  File "LabelsForWindows\bin\LabelsForWindows.dll"
  File "LabelsForWindows\bin\SharpShell.dll"

  Exec '"$INSTDIR\ComRegTool.exe" install register "$INSTDIR\LabelsForWindows.dll"'

  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\Labels for Windows" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Labels for Windows" "DisplayName" "Labels for Windows"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Labels for Windows" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Labels for Windows" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Labels for Windows" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Labels for Windows"
  DeleteRegKey HKLM "SOFTWARE\Labels for Windows"

  ; Unregister COM server
  ExecWait '"$INSTDIR\ComRegTool.exe" uninstall unregister "$INSTDIR\LabelsForWindows.dll"'

  ; Remove directories used
  RMDir "$SMPROGRAMS\Labels for Windows"
  RMDir /r "$INSTDIR"

SectionEnd

