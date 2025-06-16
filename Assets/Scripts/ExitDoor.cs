using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public int direction; //0=North, 1=South, 2=East, 3=West

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager.Instance.TransitionRoom(direction);
        }
    }
}
