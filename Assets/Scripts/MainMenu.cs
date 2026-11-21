using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject confirmPanel;

    public Button[] levelButtons;   // 3 buttons
    public Text[] levelInfoTexts;   // "BEST: 5" or "LOCKED"
    public string[] levelNames = { "ROOFTOPS", "MARKET", "CONSTRUCTION" };
    public Slider volumeSlider;  // music
    public Slider sfxSlider;

    void Start()
    {
        SaveSystem.Get();
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    // PLAY opens the level list
    public void OpenLevelSelect()
    {
        RefreshLevels();
        levelSelectPanel.SetActive(true);
    }

    void RefreshLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = SaveSystem.IsUnlocked(i + 1);
            levelButtons[i].interactable = unlocked;
            if (unlocked)
                levelInfoTexts[i].text = "BEST: " + SaveSystem.data.bestFish[i] + " FISH";
            else
                levelInfoTexts[i].text = "LOCKED";
        }
    }

    public void PlayLevel(int index)
    {
        if (!SaveSystem.IsUnlocked(index + 1)) return;
        SaveSystem.selectedLevel = index;
        PlayerHealth.tries = 1; // new game, count tries again
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        volumeSlider.value = SaveSystem.data.musicVolume;
        sfxSlider.value = SaveSystem.data.sfxVolume;
        settingsPanel.SetActive(true);
    }

    public void OnVolumeChanged(float value)
    {
        SaveSystem.data.musicVolume = value;
        if (AudioManager.instance != null) AudioManager.instance.SetMusicVolume(value);
        SaveSystem.Save(); // save when settings change
    }

    public void OnSfxChanged(float value)
    {
        SaveSystem.data.sfxVolume = value;
        if (AudioManager.instance != null) AudioManager.instance.SetSfxVolume(value);
        SaveSystem.Save();
    }

    public void AskReset()
    {
        confirmPanel.SetActive(true);
    }

    public void ConfirmReset()
    {
        SaveSystem.ResetProgress();
        confirmPanel.SetActive(false);
        RefreshLevels();
    }

    public void CancelReset()
    {
        confirmPanel.SetActive(false);
    }

    public void CloseAll()
    {
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    public void QuitGame()
    {
        SaveSystem.Save();
        Debug.Log("Quit game");
        Application.Quit();
    }
}
