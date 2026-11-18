using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Text fishText;
    public Text triesText;
    public Text timerText;
    public Text levelNumberText;
    public Text levelNameText;
    public GameObject deliveredText;

    public int fish = 0;

    public void UpdateHearts(int hearts)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < hearts)
                heartImages[i].sprite = fullHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }

    public void AddFish(int amount)
    {
        fish += amount;
        fishText.text = fish.ToString();
    }

    public void ShowTries(int tries)
    {
        triesText.text = "TRY " + tries;
    }

    public void ShowTime(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        timerText.text = (s / 60) + ":" + (s % 60).ToString("00");
        timerText.color = s <= 10 ? new Color(1f, 0.4f, 0.4f) : Color.white; // red at the end
    }

    public void ShowLevel(int number, string levelName)
    {
        levelNumberText.text = "LEVEL " + number;
        levelNameText.text = levelName;
    }

    public void ShowDelivered(int tries)
    {
        deliveredText.SetActive(true);
        deliveredText.GetComponent<Text>().text = "DELIVERED!\nTRIES: " + tries;
    }
}
