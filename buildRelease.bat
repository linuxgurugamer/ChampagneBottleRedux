@echo off
set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

rem set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
rem echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)



set VERSIONFILE=ChampagneBottle.version
rem The following requires the JQ program, available here: https://stedolan.github.io/jq/download/
c:\local\jq-win64  ".VERSION.MAJOR" %VERSIONFILE% >tmpfile
set /P major=<tmpfile

c:\local\jq-win64  ".VERSION.MINOR"  %VERSIONFILE% >tmpfile
set /P minor=<tmpfile

c:\local\jq-win64  ".VERSION.PATCH"  %VERSIONFILE% >tmpfile
set /P patch=<tmpfile

c:\local\jq-win64  ".VERSION.BUILD"  %VERSIONFILE% >tmpfile
set /P build=<tmpfile
del tmpfile
set VERSION=%major%.%minor%.%patch%
if "%build%" NEQ "0"  set VERSION=%VERSION%.%build%

echo Version:  %VERSION%


mkdir %HOMEDIR%\install\GameData\Champagne
mkdir %HOMEDIR%\install\GameData\Champagne\Textures
mkdir %HOMEDIR%\install\GameData\Champagne\Plugins

del /q %HOMEDIR%\install\GameData\Champagne
del /q %HOMEDIR%\install\GameData\Champagne\Textures
del /q %HOMEDIR%\install\GameData\Champagne\Plugins


copy /Y "%~dp0bin\Release\ChampagneBottle.dll" "%HOMEDIR%\install\GameData\Champagne\PlugIns"
copy /Y "%~dp0GameData\Textures\*.png" "%HOMEDIR%\install\GameData\Champagne\Textures"
         
copy /Y ChampagneBottle.version "%HOMEDIR%\install\GameData\Champagne"

copy /Y "License.txt" "%HOMEDIR%\install\GameData\Champagne"
copy /Y "README.md" "%HOMEDIR%\install\GameData\Champagne"
copy /Y MiniAVC.dll  "%HOMEDIR%\install\GameData\Champagne"


copy /Y "%~dp0GameData\ChampagneBottle.cfg" "%HOMEDIR%\install\GameData\Champagne"
copy /Y "%~dp0Gamedata\*.txt" "%HOMEDIR%\install\GameData\Champagne""


%HOMEDRIVE%
cd %HOMEDIR%\install

set FILE="%RELEASEDIR%\ChampagneBottle-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\Champagne

