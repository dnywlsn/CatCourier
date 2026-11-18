using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Pigeon, Dog, Crow }
public enum PropType { AC, Chimney }

[System.Serializable]
public class RoofInfo
{
    public Sprite sprite;
    public float leftX;
    public float topY;
}

[System.Serializable]
public class PropInfo
{
    public PropType type;
    public Vector2 position;
}

[System.Serializable]
public class EnemySpawn
{
    public EnemyType type;
    public Vector2 position;
    public float leftX;  // patrol limits (pigeon and dog)
    public float rightX;
}

// one level = one asset
[CreateAssetMenu(fileName = "Level_01", menuName = "Cat Courier/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName = "ROOFTOPS";
    public float timeLimit = 90f;          // seconds to deliver the parcel
    public Color roofColor = Color.white;
    public Vector2 startPosition = new Vector2(2f, 0.2f);
    public Vector2 doorPosition = new Vector2(60f, 0f);

    public List<RoofInfo> roofs = new List<RoofInfo>();
    public List<PropInfo> props = new List<PropInfo>();
    public List<Vector2> fish = new List<Vector2>();
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
}
