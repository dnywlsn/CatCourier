using UnityEngine;

[CreateAssetMenu(fileName = "DogStats", menuName = "Cat Courier/Dog Stats")]
public class DogStats : ScriptableObject
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float viewRange = 6f;
    public float viewAngle = 60f;
    public float forgetTime = 1.5f;
    public float attackRange = 1.8f;
    public float windUpTime = 0.5f;
    public float biteSpeed = 8f;
    public float biteTime = 0.25f;
    public float cooldown = 1.2f;
}
