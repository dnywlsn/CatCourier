using UnityEngine;

// on the phone the app is "paused" when you go to home screen,
// often it is killed after that, so we save here
public class SaveOnPause : MonoBehaviour
{
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save();
    }

    void OnApplicationQuit()
    {
        SaveSystem.Save();
    }
}
