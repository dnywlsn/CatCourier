using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static int tries = 1; // static so it stays after restart

    public int maxHearts = 3;
    public int hearts;
    public float fallY = -7f;
    public float invincibleTime = 1f;
    public Vector2 knockback = new Vector2(6f, 7f);
    public GameUI ui;

    Vector3 startPos;
    bool invincible = false;
    SpriteRenderer sr;

    void Start()
    {
        hearts = maxHearts;
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        ui.UpdateHearts(hearts);
        ui.ShowTries(tries);
    }

    void Update()
    {
        // fell down between the roofs
        if (transform.position.y < fallY)
        {
            LoseHeart();
            Respawn();
        }
    }

    // put the cat back on the middle of the last roof
    void Respawn()
    {
        Collider2D roof = GetComponent<PlayerController>().lastGround;
        if (roof != null)
            transform.position = new Vector3(roof.bounds.center.x, roof.bounds.max.y + 0.1f, 0);
        else
            transform.position = startPos;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        StartCoroutine(Blink());
    }

    // enemy or pebble hit the cat
    public void TakeHit(Vector3 fromPos)
    {
        if (invincible) return;
        LoseHeart();
        float side = transform.position.x < fromPos.x ? -1 : 1;
        GetComponent<PlayerController>().Knockback(new Vector2(knockback.x * side, knockback.y));
        StartCoroutine(Blink());
    }

    void LoseHeart()
    {
        hearts--;
        ui.UpdateHearts(hearts);
        if (hearts <= 0)
        {
            tries++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator Blink()
    {
        invincible = true;
        float t = 0;
        while (t < invincibleTime)
        {
            sr.color = new Color(1, 1, 1, 0.3f);
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            t += 0.2f;
        }
        invincible = false;
    }
}
