using UnityEngine;

// simple sprite animation without animator
public class CatAnimation : MonoBehaviour
{
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    public Sprite jumpFrame;
    public float frameTime = 0.1f;

    SpriteRenderer sr;
    PlayerController player;
    float timer;
    int frame;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer = 0;
            frame++;
        }

        if (!player.isGrounded)
            sr.sprite = jumpFrame;
        else if (player.moveInput != 0)
            sr.sprite = runFrames[frame % runFrames.Length];
        else
            sr.sprite = idleFrames[(frame / 5) % idleFrames.Length];
    }
}
