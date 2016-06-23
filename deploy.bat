rem unusable line


set H=R:\KSP_1.1.3_dev
echo %H%

set d=%H%
if exist %d% goto one
mkdir %d%
:one
set d=%H%\Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=%H%\Gamedata\Champagne
if exist %d% goto three
mkdir %d%
:three
set d=%H%\Gamedata\Champagne\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=%H%\Gamedata\Champagne\Textures
if exist %d% goto five
mkdir %d%
:five



copy /Y "%~dp0bin\Debug\ChampagneBottle.dll" "%H%\GameData\Champagne\Plugins"
copy /Y "%~dp0GameData\Textures\*.png" "%H%\GameData\Champagne\Textures"
copy /Y "%~dp0ChampagneBottle.version" "%H%\GameData\Champagne"

copy /Y "%~dp0License.txt" "%H%\GameData\Champagne"
copy /Y "%~dp0README.md" "%H%\GameData\Champagne"

copy /Y "%~dp0GameData\ChampagneBottle.cfg" "%H%\GameData\Champagne"
copy /Y "%~dp0Gamedata\*.txt" "%H%\GameData\Champagne"
