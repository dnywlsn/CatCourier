using UnityEngine;

// shows level name at the start and then hides it
public class LevelTitle : MonoBehaviour
{
    public float showTime = 2f;

    void Start()
    {
        Destroy(gameObject, showTime);
    }
}
