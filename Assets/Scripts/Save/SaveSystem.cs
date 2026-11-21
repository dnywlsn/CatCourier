using System.IO;
using UnityEngine;

// saves progress to a JSON file on the phone
public static class SaveSystem
{
    public static SaveData data;
    public static int selectedLevel = 0; // index of the level we play now (0, 1, 2)

    public static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "save.json"); }
    }

    public static SaveData Get()
    {
        if (data == null) Load();
        return data;
    }

    public static void Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log("No save file, start with defaults");
                data = new SaveData();
                return;
            }
            string json = File.ReadAllText(FilePath);
            data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) data = new SaveData();
        }
        catch (System.Exception e)
        {
            // broken file -> don't crash, just use defaults
            Debug.LogWarning("Save file is broken, using defaults: " + e.Message);
            data = new SaveData();
        }
        FixData();
    }

    // if some fields are missing in the file, put default values back
    static void FixData()
    {
        SaveData d = new SaveData();
        if (data.unlockedLevels == null || data.unlockedLevels.Count == 0) data.unlockedLevels = d.unlockedLevels;
        if (!data.unlockedLevels.Contains(1)) data.unlockedLevels.Add(1);
        if (data.bestFish == null) data.bestFish = d.bestFish;
        while (data.bestFish.Count < 3) data.bestFish.Add(0);
        if (data.musicVolume < 0 || data.musicVolume > 1) data.musicVolume = d.musicVolume;
        if (data.sfxVolume < 0 || data.sfxVolume > 1) data.sfxVolume = d.sfxVolume;
    }

    public static void Save()
    {
        if (data == null) return;
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath, json);
            Debug.Log("Saved to " + FilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not save: " + e.Message);
        }
    }

    public static bool IsUnlocked(int levelNumber)
    {
        return Get().unlockedLevels.Contains(levelNumber);
    }

    // called by the door at the end of a level
    public static void CompleteLevel(int levelIndex, int fish)
    {
        SaveData d = Get();
        d.totalDeliveries++;
        if (fish > d.bestFish[levelIndex]) d.bestFish[levelIndex] = fish;
        int next = levelIndex + 2; // next level number
        if (next <= 3 && !d.unlockedLevels.Contains(next)) d.unlockedLevels.Add(next);
        Save();
    }

    // reset progress but keep the settings
    public static void ResetProgress()
    {
        float music = Get().musicVolume;
        float sfx = Get().sfxVolume;
        data = new SaveData();
        data.musicVolume = music;
        data.sfxVolume = sfx;
        Save();
    }
}
