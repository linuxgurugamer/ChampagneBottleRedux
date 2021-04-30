rem unusable line

set H=%KSPDIR%
set GAMEDIR=Champagne

echo %H%

copy /Y "%1%2" "GameData\%GAMEDIR%\Plugins"
copy /Y "%1%3".pdb "GameData\%GAMEDIR%\Plugins"

copy /Y "License.txt" "GameData\%GAMEDIR%"
copy /Y "README.md" "GameData\%GAMEDIR%"
copy /Y "ChampagneBottle.version" "GameData\%GAMEDIR%"

set DP0=r:\dp0\kspdev

mkdir "%H%\GameData\%GAMEDIR%"
xcopy  /E /y /I GameData\%GAMEDIR% "%H%\GameData\%GAMEDIR%"
copy /Y "License.txt" "%H%\GameData\Champagne"
copy /Y "README.md" "%H%\GameData\Champagne"
xcopy /y /s /I GameData\%GAMEDIR% "%DP0%\GameData\%GAMEDIR%"


REM pause