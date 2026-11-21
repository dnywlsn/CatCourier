using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDoor : MonoBehaviour
{
    public float waitBeforeNext = 2.5f;
    bool done = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!done && other.GetComponent<PlayerController>() != null)
        {
            done = true;
            GameUI ui = FindFirstObjectByType<GameUI>();
            ui.ShowDelivered(PlayerHealth.tries);
            AudioManager.Play(Sfx.Win);

            // save right after the level
            SaveSystem.CompleteLevel(SaveSystem.selectedLevel, ui.fish);
            StartCoroutine(GoNext());
        }
    }

    IEnumerator GoNext()
    {
        yield return new WaitForSecondsRealtime(waitBeforeNext);
        PlayerHealth.tries = 1;
        if (SaveSystem.selectedLevel < 2)
        {
            SaveSystem.selectedLevel++;
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); // last level done
        }
    }
}
