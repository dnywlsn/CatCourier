using UnityEngine;

[CreateAssetMenu(fileName = "PigeonStats", menuName = "Cat Courier/Pigeon Stats")]
public class PigeonStats : ScriptableObject
{
    public float flySpeed = 2.5f;
    public float seeRange = 4.5f;
    public float windUpTime = 0.5f;
    public float diveSpeed = 7f;
    public float returnSpeed = 3f;
    public float cooldown = 1.5f;
}
