using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats; // speed, jump etc. are in the data asset

    public MobileButton leftButton;
    public MobileButton rightButton;
    public MobileButton jumpButton;

    public bool isGrounded;
    public float moveInput;
    public Collider2D lastGround; // last roof the cat stood on

    Rigidbody2D rb;
    SpriteRenderer sr;
    bool jumpWasPressed;
    float knockTimer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = 0;
        if (leftButton.isPressed || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1;
        if (rightButton.isPressed || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveInput = 1;

        // jump only once per press
        bool jumpNow = jumpButton.isPressed || Input.GetKey(KeyCode.Space);
        if (jumpNow && !jumpWasPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            isGrounded = false;
            AudioManager.Play(Sfx.Jump);
        }
        jumpWasPressed = jumpNow;

        // turn the cat
        if (moveInput < 0) sr.flipX = true;
        if (moveInput > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        // after a hit the cat flies back a little, no control
        if (knockTimer > 0)
        {
            knockTimer -= Time.fixedDeltaTime;
            return;
        }
        rb.linearVelocity = new Vector2(moveInput * stats.speed, rb.linearVelocity.y);
    }

    public void Knockback(Vector2 force)
    {
        rb.linearVelocity = force;
        knockTimer = stats.knockbackTime;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                lastGround = collision.collider;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
