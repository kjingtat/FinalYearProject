using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep crosshair on 2D plane
        transform.position = mousePosition;
    }
}
