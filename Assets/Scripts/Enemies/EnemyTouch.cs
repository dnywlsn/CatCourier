using UnityEngine;

// when the cat touches an enemy: jump on top = enemy is gone, from the side = cat gets hurt
public class EnemyTouch : MonoBehaviour
{
    public float bounceForce = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp == null) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        bool fromAbove = rb.linearVelocity.y < -0.1f && other.transform.position.y > transform.position.y + 0.3f;

        if (fromAbove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            Destroy(gameObject);
        }
        else
        {
            hp.TakeHit(transform.position);
        }
    }
}
