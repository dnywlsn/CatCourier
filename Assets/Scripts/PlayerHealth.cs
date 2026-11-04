using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    public int hearts;
    public float fallY = -7f;
    public GameUI ui;

    Vector3 startPos;

    void Start()
    {
        hearts = maxHearts;
        startPos = transform.position;
        ui.UpdateHearts(hearts);
    }

    void Update()
    {
        // fell down between the roofs
        if (transform.position.y < fallY)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        hearts--;
        ui.UpdateHearts(hearts);

        if (hearts <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        transform.position = startPos;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
