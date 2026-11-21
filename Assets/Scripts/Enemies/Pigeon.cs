using UnityEngine;

// Pigeon: flies left-right, sees the cat below, waits a bit and dives
public class Pigeon : MonoBehaviour
{
    enum State { Patrol, WindUp, Dive, Return }
    State state = State.Patrol;

    public PigeonStats stats;   // numbers are in the data asset
    public float leftX = 0f;    // patrol limits come from LevelData
    public float rightX = 5f;

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
                if (cooldownTimer <= 0 && below && dx < stats.seeRange && Vector2.Distance(player.position, transform.position) < stats.seeRange + 2f)
                {
                    state = State.WindUp;
                    timer = stats.windUpTime;
                    alertIcon.SetActive(true);
                    AudioManager.Play(Sfx.PigeonWindUp); // sound at the start of the wind-up
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
                transform.position = Vector3.MoveTowards(transform.position, diveTarget, stats.diveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, diveTarget) < 0.1f)
                {
                    state = State.Return;
                }
                break;

            case State.Return:
                Vector3 back = new Vector3(transform.position.x, patrolY, 0);
                transform.position = Vector3.MoveTowards(transform.position, back, stats.returnSpeed * Time.deltaTime);
                if (Mathf.Abs(transform.position.y - patrolY) < 0.05f)
                {
                    cooldownTimer = stats.cooldown;
                    state = State.Patrol;
                }
                break;
        }
    }

    void Fly()
    {
        transform.position += Vector3.right * dir * stats.flySpeed * Time.deltaTime;
        if (transform.position.x > rightX) dir = -1;
        if (transform.position.x < leftX) dir = 1;
        sr.flipX = dir < 0;
        sr.sprite = flyFrames[(int)(Time.time * 8) % flyFrames.Length];
    }
}
