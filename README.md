# BB2Stats
 "Blood Bowl 2" real time stats overlay panels designed to be used for live streaming and give add value to your streaming.
  
![BB2 Stats capture image](https://github.com/vsirvent/BB2Stats/blob/main/BB2Stats/images/capture.jpg?raw=true)

## Compilation

Just get repository and compile it using Visual Studio (tested on 2022) version. 

You can get a compiled stand-alone version here: 

* [BB2Stats v1.0.0.2](https://github.com/vsirvent/BB2Stats/blob/main/BB2Stats/releases/BB2Stats_1_0_0_2.zip)

## Usage

Just press "TAB" button while playing to show the game stats.

NOTE: NPCAP package is required to be installed in the computer, as BB2Stats sniffs incoming packets while playing a game to generate game stats.
Get and install NPCAP from repo: https://github.com/nmap/npcap

### Manual stats

Use the main stats panel to add manually stats to the application. This can be only done if there is a background person monitorinzing the game and adding the stats. Then caster can get the stats in real time by connecting to the same "Session" (check settings).

## Automatic stats

To use automatic stats go to settings and activate "Sniff" option, select the ethernet device you are using for internet connection and "Apply". To check that sniffer is working the frame counter located at the right side of the panel must increase. The person filling the stats must PUBLISH and the caster that reads it must be SUBSCRIBER. 

## Settings

### Session

This is used to share stats between 2 BB2Stats applications running in different devices, useful when using manual stats and one person manages the stats in background and the caster shows it, as it's very difficult to fill the stats manually and cast a game at the same time.

### Sniffer

Activate it and select the ethernet network device to get automatic stats from the game.

