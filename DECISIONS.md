# DECISIONS.md - Cat Courier

What I asked the AI agent, what it got wrong, what I changed and why.

## Week 10 - Three enemy types

2026-11-11  Chose 3 enemies for my rooftop game: pigeon (dives from the sky), dog (patrol + chase on a roof), crow (throws pebbles). Asked the agent (Claude) to make the art, the 3 state machines and place them in Level 1.
2026-11-11  Agent's first Dog got stuck at the edge of its roof and never turned around (Clamp kept x inside the limits, so "x > rightX" was never true). Changed the check to >= and <=.
2026-11-11  First pigeon patrolled right above the first gap and dived while the cat was jumping, so the cat always fell in the first 5 seconds. Pigeon1 leftX 9 -> 11.5.
2026-11-11  Falling between roofs sent the cat back to the very start - too harsh with enemies. Now the cat goes back to the last roof it stood on (lastGround) and blinks for 1 s.
2026-11-11  Added a "TRY N" counter so tries are easy to count in the playtest.

## Week 11 - Data and saves

2026-11-18  Asked the agent (Claude) to move every balance number out of the scripts into ScriptableObjects: PlayerStats, PigeonStats, DogStats, CrowStats, ItemValues (Assets/Data). Now I can change speed or jump in the Inspector during Play mode without touching code.
2026-11-18  Level 1 was built by hand in the scene, so it could not be reused. Now the scene "Game" is empty and LevelBuilder spawns roofs, props, fish, enemies and the door from a LevelData asset. 3 levels: ROOFTOPS, MARKET, CONSTRUCTION. Same scene code for all of them.
2026-11-18  Save is JSON (JsonUtility) in Application.persistentDataPath/save.json, not BinaryFormatter. JSON is easy to read and to break on purpose for the test.
2026-11-18  Load is inside try/catch. Tested: cut the file in half -> warning in Console, game starts with default progress, no crash. Missing file -> defaults too.
2026-11-18  Save happens after a level is delivered, when the volume slider changes, in OnApplicationPause(true) and on quit.
2026-11-18  Reset progress asks "ARE YOU SURE?" first. I decided reset keeps the music volume, because it is a setting and not progress.
2026-11-18  Bug: when I opened a level from the menu, the HUD (hearts, timer, jump button) was in the wrong place. SafeArea only updated when the safe area changed, not when the screen size changed after loading the scene. Now it checks the screen size too.
2026-11-18  Every level now has a time limit from LevelData (90 / 100 / 110 s). Timer turns red in the last 10 seconds.

## Week 12 - Sound pass

2026-11-21  Asked the agent (Claude) for a full sound layer. I did not want sounds from random sites with unclear licenses, so all 14 sounds and 2 music loops are made by a script (Tools/make_sounds.py, square/triangle waves + noise). License CC0, listed in README.md.
2026-11-21  Mixer: Master > Music > MusicTracks and Master > SFX, exposed MusicVol and SFXVol. First plan had the Paused snapshot lower the Music group, but an exposed parameter is controlled by the script and the snapshot can't change it. So the pause changes the child group MusicTracks (-10 dB + Lowpass 900 Hz) and the slider changes Music.
2026-11-21  Pause sets Time.timeScale = 0, so the snapshot transition and the crossfade would freeze. Mixer Update Mode is Unscaled Time and the crossfade uses unscaledDeltaTime.
2026-11-21  Volume slider is 0..1 but the mixer wants dB, so Log10(value) * 20 (0.5 = -6 dB). mixer.SetFloat does not work in Awake, so the volumes are set in Start. Saved as musicVolume / sfxVolume in save.json; tested: changed SFX to -11.7 dB, restarted, still -11.7 dB.
2026-11-21  One AudioManager with DontDestroyOnLoad; it only starts a track if it is not already playing, so music does not restart on level reload or next level (checked: time kept going after reload). Repeated sounds get pitch 0.92-1.08, hit and pickup have 2 clips.
2026-11-21  The agent's editor script for making the mixer failed first (AddGroupToCurrentView crashed because the new mixer has no views). Removed that call; the groups still show in the mixer window.

## Playtest (fill in)

Tester 1: ___ tries
Tester 2: ___ tries
Changed after playtest:
- ___: ___ -> ___
- ___: ___ -> ___
