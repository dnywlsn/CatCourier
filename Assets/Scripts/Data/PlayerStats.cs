using UnityEngine;

// all numbers for the cat
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Cat Courier/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float speed = 6f;
    public float jumpForce = 13f;
    public int maxHearts = 3;
    public float invincibleTime = 1f;
    public Vector2 knockback = new Vector2(6f, 7f);
    public float knockbackTime = 0.25f;
    public float stompBounce = 10f; // jump after landing on an enemy
    public float fallY = -7f;       // below this the cat is lost
}
