# Makes all sounds and music for Cat Courier (8-bit style).
# Run: python3 make_sounds.py   (needs numpy)
# Output goes to Assets/Audio. Everything is made from simple waves,
# no samples from the internet, so the license is CC0 (free to use).
import os
import wave
import numpy as np

RATE = 22050
OUT = os.path.join(os.path.dirname(__file__), "..", "Assets", "Audio")
rng = np.random.default_rng(7)  # same noise every time


def t_of(dur):
    return np.arange(int(RATE * dur)) / RATE


def osc(freq, dur, kind="square"):
    # freq can be a number or an array (for slides)
    t = t_of(dur)
    if np.isscalar(freq):
        freq = np.full(len(t), float(freq))
    phase = np.cumsum(freq) / RATE
    if kind == "square":
        return np.where((phase % 1) < 0.5, 1.0, -1.0)
    if kind == "triangle":
        return 4 * np.abs((phase % 1) - 0.5) - 1
    if kind == "saw":
        return 2 * (phase % 1) - 1
    return np.sin(2 * np.pi * phase)


def slide(f1, f2, dur):
    return np.linspace(f1, f2, int(RATE * dur))


def noise(dur):
    return rng.uniform(-1, 1, int(RATE * dur))


def env(sound, attack=0.005, release=0.05):
    n = len(sound)
    e = np.ones(n)
    a = min(n, int(RATE * attack))
    r = min(n, int(RATE * release))
    if a > 0:
        e[:a] = np.linspace(0, 1, a)
    if r > 0:
        e[-r:] *= np.linspace(1, 0, r)
    return sound * e


def decay(sound, speed=8):
    return sound * np.exp(-speed * t_of(len(sound) / RATE))


def mix(*parts):
    n = max(len(p) for p in parts)
    out = np.zeros(n)
    for p in parts:
        out[:len(p)] += p
    return out


def join(*parts):
    return np.concatenate(parts)


def save(name, sound, peak=0.8):
    sound = sound / (np.max(np.abs(sound)) + 1e-9) * peak
    data = (sound * 32767).astype(np.int16)
    path = os.path.join(OUT, name + ".wav")
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with wave.open(path, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(RATE)
        w.writeframes(data.tobytes())
    print("saved", path, round(len(sound) / RATE, 2), "s")


def note(n):
    # midi note number -> Hz
    return 440.0 * 2 ** ((n - 69) / 12)


# ---------------- sound effects ----------------
def make_sfx():
    # jump: short square going up
    save("SFX/jump", env(osc(slide(320, 760, 0.16), 0.16) * 0.6, release=0.06))

    # hit: noise + low drop (2 versions so it does not sound robotic)
    for i, f in enumerate([220, 180]):
        s = mix(decay(noise(0.22), 18) * 0.7, env(osc(slide(f, 70, 0.22), 0.22) * 0.6))
        save("SFX/hit_%d" % (i + 1), s)

    # pickup: two quick happy notes (2 versions)
    for i, (a, b) in enumerate([(81, 88), (83, 90)]):
        s = join(env(osc(note(a), 0.06, "triangle")), env(osc(note(b), 0.12, "triangle"), release=0.08))
        save("SFX/pickup_%d" % (i + 1), s)

    # enemy wind-ups: each enemy has its own warning
    t = t_of(0.45)
    coo = osc(380 + 60 * np.sin(2 * np.pi * 9 * t) + 200 * t, 0.45, "sine")
    save("SFX/windup_pigeon", env(coo, 0.02, 0.1))

    growl = osc(85 + 10 * np.sin(2 * np.pi * 30 * t_of(0.5)), 0.5, "saw")
    growl = growl * (0.6 + 0.4 * np.sin(2 * np.pi * 14 * t_of(0.5))) + noise(0.5) * 0.15
    save("SFX/windup_dog", env(growl, 0.03, 0.1))

    caw = mix(osc(slide(760, 480, 0.3), 0.3, "square") * 0.6, noise(0.3) * 0.25)
    save("SFX/windup_crow", env(caw, 0.01, 0.12))

    # crow throws a pebble: whoosh
    w = noise(0.2) * np.linspace(0.2, 1, int(RATE * 0.2))
    w = np.convolve(w, np.ones(6) / 6, mode="same")  # softer noise
    save("SFX/throw", env(w, 0.01, 0.08), 0.6)

    # enemy defeated (stomp): falling "boing"
    s = mix(env(osc(slide(700, 120, 0.25), 0.25) * 0.6), decay(noise(0.05), 40) * 0.5)
    save("SFX/enemy_defeat", s)

    # cat falls between roofs
    save("SFX/fall", env(osc(slide(900, 160, 0.6), 0.6, "sine"), 0.01, 0.15))

    # lose: sad notes down
    s = join(*[env(osc(note(n), d, "triangle"), release=0.05) for n, d in [(67, 0.22), (64, 0.22), (60, 0.22), (55, 0.5)]])
    save("SFX/lose", s)

    # win: happy notes up
    s = join(*[env(osc(note(n), d, "square") * 0.5, release=0.04) for n, d in [(72, 0.12), (76, 0.12), (79, 0.12), (84, 0.45)]])
    save("SFX/win", s)

    # UI click
    save("SFX/click", env(osc(1300, 0.045, "square") * 0.5, 0.001, 0.03), 0.6)


# ---------------- music ----------------
def track(bpm, bars, melody, bass, lead_kind, drums):
    beat = 60.0 / bpm
    total = int(RATE * beat * 4 * bars)
    out = np.zeros(total)

    def put(start_beat, sound):
        i = int(RATE * start_beat * beat)
        sound = sound[: total - i]
        out[i:i + len(sound)] += sound

    # melody / bass: list of (midi or None, beats) repeated to fill the bars
    for notes, kind, vol in [(melody, lead_kind, 0.28), (bass, "triangle", 0.45)]:
        pos = 0.0
        while pos < bars * 4 - 0.001:
            for n, length in notes:
                if pos >= bars * 4 - 0.001:
                    break
                if n is not None:
                    put(pos, env(osc(note(n), length * beat * 0.9, kind) * vol, 0.005, 0.04))
                pos += length
    if drums:
        for b in range(bars * 4 * 2):  # hi-hat on every half beat
            put(b * 0.5, decay(noise(0.05), 60) * (0.12 if b % 2 else 0.08))
        for b in range(bars * 4):  # kick on every beat
            put(b, env(osc(slide(140, 45, 0.12), 0.12, "sine") * 0.5, 0.001, 0.05))
    return out


def make_music():
    # menu: slow and calm, A minor
    mel = [(69, 1), (72, 1), (76, 2), (74, 1), (72, 1), (71, 2),
           (69, 1), (71, 1), (72, 2), (64, 2), (None, 2),
           (69, 1), (72, 1), (76, 2), (77, 1), (76, 1), (74, 2),
           (72, 1), (71, 1), (69, 4), (None, 2)]
    bass = [(45, 4), (41, 4), (43, 4), (40, 4)]
    save("Music/menu_music", track(84, 8, mel, bass, "triangle", False), 0.7)

    # level: faster, with drums, C major
    mel = [(72, 0.5), (76, 0.5), (79, 0.5), (76, 0.5), (77, 1), (74, 1),
           (72, 0.5), (74, 0.5), (76, 1), (72, 1), (None, 1),
           (79, 0.5), (77, 0.5), (76, 0.5), (74, 0.5), (72, 1), (74, 1),
           (76, 0.5), (74, 0.5), (72, 2), (None, 1)]
    bass = [(48, 1), (48, 1), (55, 1), (48, 1), (53, 1), (53, 1), (57, 1), (53, 1),
            (55, 1), (55, 1), (59, 1), (55, 1), (48, 1), (52, 1), (55, 1), (52, 1)]
    save("Music/level_music", track(132, 16, mel, bass, "square", True), 0.7)


if __name__ == "__main__":
    make_sfx()
    make_music()
