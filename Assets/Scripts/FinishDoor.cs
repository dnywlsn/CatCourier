using UnityEngine;
using UnityEngine.UI;

public class FinishDoor : MonoBehaviour
{
    public GameObject deliveredText;
    bool done = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!done && other.GetComponent<PlayerController>() != null)
        {
            done = true;
            deliveredText.SetActive(true);
            deliveredText.GetComponent<Text>().text = "DELIVERED!\nTRIES: " + PlayerHealth.tries;
            Debug.Log("Level done in tries: " + PlayerHealth.tries);
        }
    }
}
