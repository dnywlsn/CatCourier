using UnityEngine;

// background moves slower than the camera
public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float factor = 0.5f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        float x = startPos.x + cam.position.x * factor;
        float y = startPos.y + cam.position.y;
        transform.position = new Vector3(x, y, startPos.z);
    }
}
