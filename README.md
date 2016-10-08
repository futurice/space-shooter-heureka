# Space Shooter Heureka
This is a simple arcade-style space shooter game which is going to be installed on the planetarium for the FutuParty 2016 in Heureka. 

## Tools
Developed on Unity3d pro v 5.4.1
If time allows we will do the dome calibration mapping with Omnidome (http://omnido.me/)

See more details about the calibration setup in /setup/README.md

## Dependencies
Uses unity plugins
- RTVoice for TTS support (purchased through assetstore https://www.assetstore.unity3d.com/en/#!/content/41068)
- Funnel for Syphon-server-support (download plugin from https://github.com/keijiro/Funnel)
- DOTween
- UniRX

## Graphical assets and 3d models
Developed by Futurice, but at the moment might contain some assets owned by Unity. 

## Running
- decide the resolution (how many projectors are you using?). reso currently hardcoded in the app and also to Unity>Player Settings
- build the game using unity3d
- if used with omnidome, output the syphon/funnel output to omnidome. And naturally calibrate the dome.

## TODO 
- remove all assets not used, and hopefully replace all assets with our in-house design

