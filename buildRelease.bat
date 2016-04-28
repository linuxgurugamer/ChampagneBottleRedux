@echo off
set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)


type ChampagneBottle.version
set /p VERSION= "Enter version: "


mkdir %HOMEDIR%\install\GameData\Champagne
mkdir %HOMEDIR%\install\GameData\Champagne\Textures
mkdir %HOMEDIR%\install\GameData\Champagne\Plugins

del %HOMEDIR%\install\GameData\Champagne
del %HOMEDIR%\install\GameData\Champagne\Textures
del %HOMEDIR%\install\GameData\Champagne\Plugins


copy /Y "%~dp0bin\Release\ChampagneBottle.dll" "%HOMEDIR%\install\GameData\Champagne\PlugIns"
copy /Y "%~dp0GameData\Textures\*.png" "%HOMEDIR%\install\GameData\Champagne\Textures"
copy /Y "%~dp0ChampagneBottle.version "%HOMEDIR%\install\GameData\Champagne"

copy /Y "License.txt" "%HOMEDIR%\install\GameData\Champagne"
copy /Y "README.md" "%HOMEDIR%\install\GameData\Champagne"
copy /Y MiniAVC.dll  "%HOMEDIR%\install\GameData\Champagne"


copy /Y "%~dp0ChampagneBottle.cfg" "%HOMEDIR%\install\GameData\Champagne"
copy /Y "%~dp0Gamedata\*.txt" "%HOMEDIR%\install\GameData\Champagne""


%HOMEDRIVE%
cd %HOMEDIR%\install

set FILE="%RELEASEDIR%\ChampagneBottle-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\Champagne

