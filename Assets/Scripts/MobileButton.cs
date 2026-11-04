using UnityEngine;
using UnityEngine.EventSystems;

// button that you can hold (for left, right and jump)
public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        transform.localScale = new Vector3(0.92f, 0.92f, 1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        transform.localScale = Vector3.one;
    }
}
