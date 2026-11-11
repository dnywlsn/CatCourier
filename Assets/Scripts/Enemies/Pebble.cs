using UnityEngine;

public class Pebble : MonoBehaviour
{
    public Vector2 velocity;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.Rotate(0, 0, 360f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeHit(transform.position);
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject); // hit a wall or roof
        }
    }
}
