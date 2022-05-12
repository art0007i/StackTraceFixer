# StackTraceFixer

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that removes stack traces from places where they should have never been.<br>
Not all stack traces are useless! This is why this mod removes only certain useless stack traces that do more bad than good.<br>
Currently removed StackTraces:
- Packing disposed nodes prints a massive stack trace wall
- Changing any element of world configuration (world name, max user count, access level)

## Installation
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [StackTraceFixer.dll](https://github.com/art0007i/StackTraceFixer/releases/latest/download/StackTraceFixer.dll) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Start the game. If you want to verify that the mod is working you can check your Neos logs.