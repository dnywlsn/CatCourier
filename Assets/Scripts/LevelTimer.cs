using UnityEngine;

// time to deliver the parcel, comes from LevelData
public class LevelTimer : MonoBehaviour
{
    public GameUI ui;
    public PlayerHealth player;
    float timeLeft;
    bool running = true;

    void Start()
    {
        timeLeft = LevelBuilder.current.timeLimit;
        ui.ShowTime(timeLeft);
    }

    void Update()
    {
        if (!running) return;
        if (ui.deliveredText.activeSelf) { running = false; return; }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            running = false;
            player.GameOver(); // too slow, try again
        }
        ui.ShowTime(timeLeft);
    }
}
