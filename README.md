# Cat Courier

A little orange cat delivers parcels across the rooftops of a night city.

2D pixel-art platformer for phones (iOS / Android), landscape.
Game Development Fundamentals, RK2 (Assignments 8-12), Aizhan Sagatova.

## Controls
- Left / right buttons (bottom-left) - move
- JUMP button (bottom-right) - jump
- Pause button (top-right) - pause menu

On the laptop: A / D or arrows to move, Space to jump, Esc to pause.

## Scenes
- `Assets/Scenes/MainMenu` - title, level select, settings (music / sound effects volume, reset progress)
- `Assets/Scenes/Game` - one scene for all 3 levels, built from `LevelData` assets

## Sound (week 12)
- `Assets/Audio/MainMixer.mixer` - groups Master > Music > MusicTracks, Master > SFX.
  Exposed volumes: MasterVol, MusicVol, SFXVol. Snapshots: Normal and Paused (music -10 dB + lowpass).
- `Assets/Scripts/Audio/AudioManager.cs` - one object with DontDestroyOnLoad: music crossfade (1 s),
  sound effects with random pitch 0.92-1.08, pause snapshot, volume in dB (Log10 x 20).
- `Assets/Data/SoundBank.asset` - which clip(s) and volume for every sound event.

### Sound sources and licenses
All sounds and both music tracks were made for this project with the script `Tools/make_sounds.py`
(numpy, simple square / triangle / sine waves and noise). No samples from other people are used.
The script was written with help of an AI agent (Claude), I chose the sounds and the volumes.
License: CC0 (public domain), free to use.

| File | Used for | Source | License |
|---|---|---|---|
| `Audio/SFX/jump.wav` | cat jumps | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/hit_1.wav`, `hit_2.wav` | cat gets hit (random one) | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/pickup_1.wav`, `pickup_2.wav` | fish picked up (random one) | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/windup_pigeon.wav` | pigeon wind-up before the dive | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/windup_dog.wav` | dog growl before the bite | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/windup_crow.wav` | crow caw when it starts to aim | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/throw.wav` | crow throws a pebble | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/enemy_defeat.wav` | enemy defeated (jump on it) | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/fall.wav` | cat falls between roofs | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/lose.wav` | no hearts / time is over | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/win.wav` | parcel delivered | made with Tools/make_sounds.py | CC0 |
| `Audio/SFX/click.wav` | every UI button | made with Tools/make_sounds.py | CC0 |
| `Audio/Music/menu_music.wav` | menu music (84 bpm loop) | made with Tools/make_sounds.py | CC0 |
| `Audio/Music/level_music.wav` | level music (132 bpm loop) | made with Tools/make_sounds.py | CC0 |

Art and pixel font: made for this project.
