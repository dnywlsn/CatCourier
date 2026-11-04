using UnityEngine;

public class Fish : MonoBehaviour
{
    public float bobSpeed = 3f;
    public float bobHeight = 0.15f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // fish goes up and down a little
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            FindFirstObjectByType<GameUI>().AddFish();
            Destroy(gameObject);
        }
    }
}
