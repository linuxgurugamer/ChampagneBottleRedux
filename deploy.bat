

mkdir R:\KSP_1.0.5_Dev\GameData\Champagne
mkdir R:\KSP_1.0.5_Dev\GameData\Champagne\Textures
mkdir R:\KSP_1.0.5_Dev\GameData\Champagne\Plugins



copy /Y "%~dp0bin\Debug\ChampagneBottle.dll" "R:\KSP_1.0.5_Dev\GameData\Champagne\Plugins"
copy /Y "%~dp0GameData\Textures\*.png" "R:\KSP_1.0.5_Dev\GameData\Champagne\Textures"
copy /Y "%~dp0ChampagneBottle.version" "R:\KSP_1.0.5_Dev\GameData\Champagne"

copy /Y "%~dp0License.txt" "R:\KSP_1.0.5_Dev\GameData\Champagne"
copy /Y "%~dp0README.md" "R:\KSP_1.0.5_Dev\GameData\Champagne"

copy /Y "%~dp0ChampagneBottle.cfg" "R:\KSP_1.0.5_Dev\GameData\Champagne"
copy /Y "%~dp0Gamedata\*.txt" "R:\KSP_1.0.5_Dev\GameData\Champagne"
