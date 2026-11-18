using UnityEngine;

public class Fish : MonoBehaviour
{
    public ItemValues values;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // fish goes up and down a little
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * values.fishBobSpeed) * values.fishBobHeight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            FindFirstObjectByType<GameUI>().AddFish(values.fishPoints);
            Destroy(gameObject);
        }
    }
}
