# Space Shooter Heureka

This is a simple arcade-style space shooter game which was installed on the Heureka Science Center planetarium for the FutuParty 2016. 

## Tools

Developed on Unity3D pro v5.4.1.
Planetarium dome projection mapping was done using [Omnidome](http://omnido.me/).

See more details about the calibration setup in /setup/README.md

## Required Dependencies

The following dependencies are required for the project to work and have open source licenses. The dependencies are included with the project under the Assets/Plugins folder.  

- [Funnel](https://github.com/keijiro/Funnel) for Syphon-server-support ([license](https://github.com/keijiro/Funnel))
- [DOTween](http://dotween.demigiant.com) ([licence](http://dotween.demigiant.com/license.php))
- [UniRX](https://github.com/neuecc/UniRx) ([license](https://github.com/neuecc/UniRx/blob/master/LICENSE))

## Optional Dependencies

The following plugins improve the game experience, but are not open source and might require purchasing.

- [RTVoice](https://www.assetstore.unity3d.com/en/#!/content/41068) for TTS support.

## Graphical assets and 3d models

Mostly developed by Futurice, but there are few remaining assets released by Unity as part of their demo assets, which are licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0) license.

## Running

- Decide the resolution (how many projectors are you using?). 
    - Resolution of the application is currently hardcoded in the app and also to *Unity > Player Settings*
    - Current resolution is 1920x2160 (2 x FullHD on top of each other)
- Build the game using Unity3d
- if used with omnidome, output the syphon/funnel output to omnidome. And naturally calibrate the dome.
    - Currently Syphon is always enabled in the final build using a platform #define directive

