using UnityEngine;

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
        }
    }
}
