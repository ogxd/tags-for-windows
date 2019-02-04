# Labels-For-Windows
MacOS style labels for Windows !

# WIP
SharpShell is not working well on my current setup (if not working at all).
However, interface for Overlay Icons is good.
Two COM registrations must be done (manually), one for x32, one for x64.
The x32 one is required even on x64 systems since some file browsers are running x32.
Alone, the x32 alone won't work for x64 file browsers such as explorer.exe.
COM attributes but also be set manually.
## x32
`C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe`
## x64
`C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe`
