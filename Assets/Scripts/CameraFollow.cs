using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;
    public float offsetY = 1.5f;
    public float minX = 6f;
    public float minY = 1.5f;

    void LateUpdate()
    {
        float x = Mathf.Max(target.position.x + 3f, minX);
        float y = Mathf.Max(target.position.y + offsetY, minY);
        Vector3 newPos = new Vector3(x, y, -10f);
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
    }
}
