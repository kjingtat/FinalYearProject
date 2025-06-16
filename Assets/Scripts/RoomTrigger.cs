using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private RoomBehavior room;

    void Start()
    {
        room = GetComponentInParent<RoomBehavior>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.OnPlayerEnterRoom();
        }
    }
}
