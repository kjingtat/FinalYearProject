using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private void LateUpdate()
    {
        if (RoomManager.Instance == null || RoomManager.Instance.currentRoom == null)
            return;

        Vector3 targetPosition = RoomManager.Instance.currentRoom.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
