using UnityEngine;

[CreateAssetMenu(fileName = "CrowStats", menuName = "Cat Courier/Crow Stats")]
public class CrowStats : ScriptableObject
{
    public float throwRange = 8f;
    public float aimTime = 0.7f;
    public float cooldown = 1.8f;
    public float pebbleSpeed = 6f;
    public float pebbleLifeTime = 3f;
}
