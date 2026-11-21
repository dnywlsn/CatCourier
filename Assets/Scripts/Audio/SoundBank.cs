using System.Collections.Generic;
using UnityEngine;

// all sound events of the game
public enum Sfx { Jump, Hit, Pickup, PigeonWindUp, DogWindUp, CrowWindUp, Throw, EnemyDefeat, Fall, Lose, Win, Click }

[System.Serializable]
public class SoundEntry
{
    public Sfx type;
    public AudioClip[] clips;          // 2-3 clips = random one each time
    [Range(0f, 1f)] public float volume = 1f;
    public bool randomPitch = true;    // small pitch change so it is not robotic
}

// clips and their volume are data, like the stats last week
[CreateAssetMenu(fileName = "SoundBank", menuName = "Cat Courier/Sound Bank")]
public class SoundBank : ScriptableObject
{
    public float minPitch = 0.92f;
    public float maxPitch = 1.08f;
    public List<SoundEntry> sounds = new List<SoundEntry>();

    public SoundEntry Find(Sfx type)
    {
        foreach (SoundEntry s in sounds)
        {
            if (s.type == type) return s;
        }
        return null;
    }
}
