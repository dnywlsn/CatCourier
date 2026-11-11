using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Text fishText;
    public Text triesText;

    int fish = 0;

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

    public void AddFish()
    {
        fish++;
        fishText.text = fish.ToString();
    }

    public void ShowTries(int tries)
    {
        triesText.text = "TRY " + tries;
    }
}
