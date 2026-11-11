using UnityEngine;

// Pigeon: flies left-right, sees the cat below, waits a bit and dives
public class Pigeon : MonoBehaviour
{
    enum State { Patrol, WindUp, Dive, Return }
    State state = State.Patrol;

    [Header("Patrol")]
    public float leftX = 0f;
    public float rightX = 5f;
    public float flySpeed = 2.5f;

    [Header("Attack")]
    public float seeRange = 4.5f;   // how far it sees the cat
    public float windUpTime = 0.5f; // red flash before dive
    public float diveSpeed = 7f;
    public float returnSpeed = 3f;
    public float cooldown = 1.5f;

    [Header("Sprites")]
    public Sprite[] flyFrames;
    public Sprite diveSprite;
    public GameObject alertIcon;

    Transform player;
    SpriteRenderer sr;
    float patrolY;
    int dir = 1;
    float timer;
    float cooldownTimer;
    Vector3 diveTarget;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        sr = GetComponent<SpriteRenderer>();
        patrolY = transform.position.y;
        alertIcon.SetActive(false);
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Patrol:
                Fly();
                // cat is below me and close enough
                float dx = Mathf.Abs(player.position.x - transform.position.x);
                bool below = player.position.y < transform.position.y;
                if (cooldownTimer <= 0 && below && dx < seeRange && Vector2.Distance(player.position, transform.position) < seeRange + 2f)
                {
                    state = State.WindUp;
                    timer = windUpTime;
                    alertIcon.SetActive(true);
                }
                break;

            case State.WindUp:
                // blink red so the player can see it
                timer -= Time.deltaTime;
                sr.color = (Mathf.Sin(Time.time * 40f) > 0) ? Color.red : Color.white;
                if (timer <= 0)
                {
                    diveTarget = player.position + Vector3.up * 0.5f;
                    sr.color = Color.white;
                    sr.sprite = diveSprite;
                    alertIcon.SetActive(false);
                    state = State.Dive;
                }
                break;

            case State.Dive:
                transform.position = Vector3.MoveTowards(transform.position, diveTarget, diveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, diveTarget) < 0.1f)
                {
                    state = State.Return;
                }
                break;

            case State.Return:
                Vector3 back = new Vector3(transform.position.x, patrolY, 0);
                transform.position = Vector3.MoveTowards(transform.position, back, returnSpeed * Time.deltaTime);
                if (Mathf.Abs(transform.position.y - patrolY) < 0.05f)
                {
                    cooldownTimer = cooldown;
                    state = State.Patrol;
                }
                break;
        }
    }

    void Fly()
    {
        transform.position += Vector3.right * dir * flySpeed * Time.deltaTime;
        if (transform.position.x > rightX) dir = -1;
        if (transform.position.x < leftX) dir = 1;
        sr.flipX = dir < 0;
        sr.sprite = flyFrames[(int)(Time.time * 8) % flyFrames.Length];
    }
}
