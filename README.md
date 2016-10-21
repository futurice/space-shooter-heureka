# Space Shooter Heureka

This is a simple arcade-style space shooter game which is going to be installed on the planetarium for the FutuParty 2016 in Heureka. 

## Tools

Developed on Unity3D pro v5.4.1.
If time allows we will do the dome calibration mapping with [Omnidome](http://omnido.me/).

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

Developed by Futurice, but at the moment might contain some assets owned by Unity. 

## Running

- Decide the resolution (how many projectors are you using?). reso currently hardcoded in the app and also to Unity>Player Settings
- Build the game using unity3d
- if used with omnidome, output the syphon/funnel output to omnidome. And naturally calibrate the dome.

## TODO 
- Remove all assets not used, and hopefully replace all assets with our in-house design

