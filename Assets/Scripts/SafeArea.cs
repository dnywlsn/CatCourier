using UnityEngine;

// script from the lecture - keeps buttons away from the notch
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform panel;
    Rect last;
    Vector2Int lastSize;

    void Awake() { panel = GetComponent<RectTransform>(); Apply(); }
    void Update()
    {
        // also check screen size - after loading a scene it can change
        if (Screen.safeArea != last || Screen.width != lastSize.x || Screen.height != lastSize.y) Apply();
    }

    void Apply()
    {
        last = Screen.safeArea;
        lastSize = new Vector2Int(Screen.width, Screen.height);
        Vector2 min = last.position;
        Vector2 max = last.position + last.size;
        min.x /= Screen.width;  min.y /= Screen.height;
        max.x /= Screen.width;  max.y /= Screen.height;
        panel.anchorMin = min;
        panel.anchorMax = max;
    }
}
