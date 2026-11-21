using System.Collections.Generic;

// everything that goes into save.json
[System.Serializable]
public class SaveData
{
    public int version = 1;
    public List<int> unlockedLevels = new List<int>() { 1 }; // level numbers 1, 2, 3
    public List<int> bestFish = new List<int>() { 0, 0, 0 };  // best fish for each level
    public int totalDeliveries = 0;
    public float musicVolume = 0.8f;
    public float sfxVolume = 0.8f;
}
