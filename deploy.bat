rem unusable line

set H=R:\KSP_1.3.1_dev
set GAMEDIR=Champagne

echo %H%

copy /Y "%1%2" "GameData\%GAMEDIR%\Plugins"
copy /Y "License.txt" "GameData\%GAMEDIR%"
copy /Y "README.md" "GameData\%GAMEDIR%"
copy /Y "ChampagneBottle.version" "GameData\%GAMEDIR%"

mkdir "%H%\GameData\%GAMEDIR%"
xcopy  /E /y /I GameData\%GAMEDIR% "%H%\GameData\%GAMEDIR%"
copy /Y "License.txt" "%H%\GameData\Champagne"
copy /Y "README.md" "%H%\GameData\Champagne"
