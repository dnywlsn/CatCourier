using UnityEngine;

// Dog: walks on its roof, looks for the cat with a raycast, chases and bites
public class Dog : MonoBehaviour
{
    enum State { Patrol, Chase, WindUp, Bite, Cooldown }
    State state = State.Patrol;

    public DogStats stats;      // numbers are in the data asset
    public float leftX = 0f;    // roof limits come from LevelData
    public float rightX = 5f;

    [Header("Sprites")]
    public Sprite[] walkFrames;
    public Sprite attackSprite;
    public GameObject alertIcon;

    Transform player;
    SpriteRenderer sr;
    int dir = 1;
    float timer;
    float lastSeenTime = -99f;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        sr = GetComponent<SpriteRenderer>();
        alertIcon.SetActive(false);
    }

    void Update()
    {
        bool sees = CanSeePlayer();
        if (sees) lastSeenTime = Time.time;
        float dist = Mathf.Abs(player.position.x - transform.position.x);

        switch (state)
        {
            case State.Patrol:
                Walk(stats.walkSpeed);
                // turn around at the edge of the roof
                if (transform.position.x >= rightX) dir = -1;
                if (transform.position.x <= leftX) dir = 1;
                if (sees) state = State.Chase;
                break;

            case State.Chase:
                dir = player.position.x > transform.position.x ? 1 : -1;
                Walk(stats.runSpeed);
                if (Time.time - lastSeenTime > stats.forgetTime)
                {
                    state = State.Patrol;
                }
                else if (sees && dist < stats.attackRange)
                {
                    state = State.WindUp;
                    timer = stats.windUpTime;
                    alertIcon.SetActive(true);
                }
                break;

            case State.WindUp:
                // stop and blink red before the bite
                timer -= Time.deltaTime;
                sr.color = (Mathf.Sin(Time.time * 40f) > 0) ? Color.red : Color.white;
                if (timer <= 0)
                {
                    sr.color = Color.white;
                    sr.sprite = attackSprite;
                    alertIcon.SetActive(false);
                    timer = stats.biteTime;
                    state = State.Bite;
                }
                break;

            case State.Bite:
                Move(stats.biteSpeed);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = stats.cooldown;
                    state = State.Cooldown;
                }
                break;

            case State.Cooldown:
                timer -= Time.deltaTime;
                sr.sprite = walkFrames[0];
                if (timer <= 0) state = State.Patrol;
                break;
        }
    }

    bool CanSeePlayer()
    {
        Vector2 eyes = transform.position + Vector3.up * 0.6f;
        Vector2 toPlayer = (Vector2)player.position + Vector2.up * 0.5f - eyes;

        // 1. distance
        if (toPlayer.magnitude > stats.viewRange) return false;
        // 2. angle (only in front of the dog)
        if (Vector2.Angle(Vector2.right * dir, toPlayer) > stats.viewAngle) return false;
        // 3. line of sight, walls block it
        RaycastHit2D[] hits = Physics2D.RaycastAll(eyes, toPlayer.normalized, toPlayer.magnitude);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.isTrigger) continue; // skip fish and enemies
            if (hit.collider.GetComponent<PlayerController>() != null)
            {
                Debug.DrawLine(eyes, hit.point, Color.yellow);
                return true;
            }
            Debug.DrawLine(eyes, hit.point, Color.red);
            return false; // wall is first
        }
        return false;
    }

    void Walk(float speed)
    {
        Move(speed);
        sr.sprite = walkFrames[(int)(Time.time * 6) % walkFrames.Length];
    }

    void Move(float speed)
    {
        float x = transform.position.x + dir * speed * Time.deltaTime;
        x = Mathf.Clamp(x, leftX, rightX); // don't fall from the roof
        transform.position = new Vector3(x, transform.position.y, 0);
        sr.flipX = dir < 0;
    }
}
