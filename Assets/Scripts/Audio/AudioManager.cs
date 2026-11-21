using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// one object for all sounds and music, it stays between scenes
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Mixer")]
    public AudioMixer mixer;
    public AudioMixerGroup musicGroup;   // Master > Music > MusicTracks
    public AudioMixerGroup sfxGroup;     // Master > SFX
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot pausedSnapshot;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public float crossfadeTime = 1f;

    [Header("Sounds")]
    public SoundBank bank;
    public int sfxSourceCount = 8;

    AudioSource musicA;
    AudioSource musicB;
    AudioSource currentMusic;
    AudioSource[] sfxSources;
    int nextSfx = 0;
    Coroutine fade;

    void Awake()
    {
        // only one AudioManager, the one from the first scene
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        musicA = MakeSource(musicGroup, true);
        musicB = MakeSource(musicGroup, true);
        currentMusic = musicA;

        sfxSources = new AudioSource[sfxSourceCount];
        for (int i = 0; i < sfxSourceCount; i++)
            sfxSources[i] = MakeSource(sfxGroup, false);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // mixer.SetFloat does not work in Awake, so here
        SaveSystem.Get();
        SetMusicVolume(SaveSystem.data.musicVolume);
        SetSfxVolume(SaveSystem.data.sfxVolume);
    }

    AudioSource MakeSource(AudioMixerGroup group, bool loop)
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.outputAudioMixerGroup = group; // every source goes to its mixer group
        s.loop = loop;
        s.playOnAwake = false;
        return s;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        normalSnapshot.TransitionTo(0f); // new scene is never paused

        if (scene.name == "MainMenu") PlayMusic(menuMusic);
        else PlayMusic(levelMusic);

        // every button in the scene makes a click
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button b in buttons)
            b.onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        Play(Sfx.Click);
    }

    // ---------- music ----------
    public void PlayMusic(AudioClip clip)
    {
        // same track already plays -> don't restart it (level reload, next level)
        if (currentMusic.clip == clip && currentMusic.isPlaying) return;

        AudioSource from = currentMusic;
        AudioSource to = (currentMusic == musicA) ? musicB : musicA;
        to.clip = clip;
        to.volume = 0f;
        to.Play();
        currentMusic = to;

        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(Crossfade(from, to));
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float t = 0;
        float fromStart = from.volume;
        while (t < crossfadeTime)
        {
            t += Time.unscaledDeltaTime; // works in pause too
            float k = t / crossfadeTime;
            from.volume = Mathf.Lerp(fromStart, 0f, k);
            to.volume = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        to.volume = 1f;
    }

    // ---------- sound effects ----------
    public static void Play(Sfx type)
    {
        if (instance != null) instance.PlaySfx(type);
    }

    void PlaySfx(Sfx type)
    {
        SoundEntry s = bank.Find(type);
        if (s == null || s.clips.Length == 0) return;

        AudioSource src = sfxSources[nextSfx];
        nextSfx = (nextSfx + 1) % sfxSources.Length;

        src.clip = s.clips[Random.Range(0, s.clips.Length)];
        src.volume = s.volume;
        src.pitch = s.randomPitch ? Random.Range(bank.minPitch, bank.maxPitch) : 1f;
        src.Play();
    }

    // ---------- pause and volume ----------
    public void SetPaused(bool paused)
    {
        // Paused snapshot: music is quieter and muffled (lowpass)
        if (paused) pausedSnapshot.TransitionTo(0.3f);
        else normalSnapshot.TransitionTo(0.3f);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVol", ToDecibels(value));
    }

    public void SetSfxVolume(float value)
    {
        mixer.SetFloat("SFXVol", ToDecibels(value));
    }

    // slider 0..1 -> dB. 1 = 0 dB, 0.5 = -6 dB, 0.1 = -20 dB
    float ToDecibels(float value)
    {
        if (value < 0.0001f) value = 0.0001f; // Log10(0) is -infinity
        return Mathf.Log10(value) * 20f;
    }
}
