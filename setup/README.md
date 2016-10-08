# Heureka setup

## Omnidome version
Omnidome version used at Heureka in 7.10.2016 was commit 2a65f9cb45e854610e2e3a80c17f6439133bb099 

github repo can be found here: https://github.com/WilstonOreo/omnidome

## Calibration
Heureka planetarium consists of two projectors each covering half of the half-dome. Projector lenses are custom-made, which made it impossible to gather any specs (such as throw-ratio) which would've helped a lot.

Calibration is saved into file HEUREKAFINAL.omni. However the version we used had bugs in loading the calibration file, f.ex the actual dome translated up several meters. It might be possible to tinker the calibration so it would work, but it is not going to be easy..

## Syphon
Frames were routed to Omnidome using Syphon, which is a client-server framework for sharing frames in real-time. Syphon only exists in OSX. Game itself doesn't put any restrictions to OS, but if you want to use Syphon, you're stuck with OSX.  

We used Funnel-plugin for Unity which worked out of the box, as long as you build x64 build. x32 or universal didn't work with that version of Funnel.

Funnel can be found here: https://github.com/keijiro/Funnel

Whole signal-processing framework is quite heavy on the GPU, so having a Mac with discrete graphics is highly recommended. 

## HW
For HW we used Macbook Pro 2,5 GHz Intel Core i7, 16 GB 1600 MHz DDR3, NVIDIA GeForce GT 750M 2048 MB, OSX El Capitan.

We had also 8 Xbox 360 wireless controllers with two wireless adapters (for Windows). Adapters work in OSX with this driver: https://github.com/360Controller/360Controller/releases. We used driver version 0.16.4

