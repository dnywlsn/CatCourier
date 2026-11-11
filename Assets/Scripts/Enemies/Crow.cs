using UnityEngine;

// Crow: sits on the roof, aims at the cat and throws a pebble
public class Crow : MonoBehaviour
{
    enum State { Idle, Aim, Throw, Cooldown }
    State state = State.Idle;

    public float throwRange = 8f;
    public float aimTime = 0.7f;
    public float cooldown = 1.8f;
    public float pebbleSpeed = 6f;

    public GameObject pebblePrefab;
    public Sprite idleSprite;
    public Sprite throwSprite;
    public GameObject alertIcon;
    public LineRenderer aimLine;

    Transform player;
    SpriteRenderer sr;
    float timer;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        sr = GetComponent<SpriteRenderer>();
        alertIcon.SetActive(false);
        aimLine.enabled = false;
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);
        sr.flipX = player.position.x > transform.position.x; // look at the cat

        switch (state)
        {
            case State.Idle:
                sr.sprite = idleSprite;
                if (dist < throwRange)
                {
                    state = State.Aim;
                    timer = aimTime;
                    alertIcon.SetActive(true);
                    aimLine.enabled = true;
                }
                break;

            case State.Aim:
                // red line shows where the pebble will go
                timer -= Time.deltaTime;
                aimLine.SetPosition(0, transform.position + Vector3.up * 0.4f);
                aimLine.SetPosition(1, player.position + Vector3.up * 0.5f);
                if (dist > throwRange)
                {
                    StopAim();
                    state = State.Idle;
                }
                else if (timer <= 0)
                {
                    StopAim();
                    state = State.Throw;
                }
                break;

            case State.Throw:
                sr.sprite = throwSprite;
                Vector3 start = transform.position + Vector3.up * 0.4f;
                Vector2 direction = (player.position + Vector3.up * 0.5f - start).normalized;
                GameObject p = Instantiate(pebblePrefab, start, Quaternion.identity);
                p.GetComponent<Pebble>().velocity = direction * pebbleSpeed;
                timer = cooldown;
                state = State.Cooldown;
                break;

            case State.Cooldown:
                timer -= Time.deltaTime;
                if (timer < cooldown - 0.3f) sr.sprite = idleSprite;
                if (timer <= 0) state = State.Idle;
                break;
        }
    }

    void StopAim()
    {
        alertIcon.SetActive(false);
        aimLine.enabled = false;
    }
}
