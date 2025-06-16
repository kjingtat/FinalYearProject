using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public GameObject player;
    public RoomBehavior currentRoom;

    private Dictionary<Vector2Int, RoomBehavior> roomMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initializes all rooms into a map and sets the starting room (currentRoom)
    public void InitializeRoomMap(RoomBehavior[] allRooms)
    {
        roomMap.Clear();

        foreach (var room in allRooms)
        {
            Vector2Int gridPos = GetRoomGridPosition(room.transform.position);
            roomMap[gridPos] = room;
        }

        // Set the starting room (currentRoom) at position (0, 0)
        if (roomMap.TryGetValue(Vector2Int.zero, out var startingRoom))
        {
            currentRoom = startingRoom;
        }
        else
        {
            Debug.LogError("Start room not found at position (0, 0)");
        }
    }

    // Converts world position to grid coordinates based on spacing and offset
    public Vector2Int GetRoomGridPosition(Vector3 worldPos)
    {
        float roomSpacingX = 18f;
        float roomSpacingY = 11f;
        Vector2 startOffset = new Vector2(0.3f, -0.1f);

        float adjustedX = worldPos.x - startOffset.x;
        float adjustedY = worldPos.y - startOffset.y;

        int gridX = Mathf.RoundToInt(adjustedX / roomSpacingX);
        int gridY = Mathf.RoundToInt(adjustedY / roomSpacingY);

        return new Vector2Int(gridX, gridY);
    }

    // Handles transition between rooms
    public void TransitionRoom(int directionIndex)
    {
        if (currentRoom == null) return;

        Vector2Int currentGridPos = GetRoomGridPosition(currentRoom.transform.position);

        Vector2Int offset = directionIndex switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.down,
            2 => Vector2Int.right,
            3 => Vector2Int.left,
            _ => Vector2Int.zero
        };

        Vector2Int newGridPos = currentGridPos + offset;

        if (roomMap.TryGetValue(newGridPos, out RoomBehavior nextRoom))
        {
            int oppositeDir = directionIndex switch
            {
                0 => 1,
                1 => 0,
                2 => 3,
                3 => 2,
                _ => directionIndex
            };

            Vector2 spawnPos = nextRoom.GetSpawnPosition(DirectionIndexToString(oppositeDir));
            player.transform.position = spawnPos;

            currentRoom = nextRoom;
        }
        else
        {
            Debug.LogWarning($"No room exists in direction index: {directionIndex}");
        }
    }

    private string DirectionIndexToString(int idx) => idx switch
    {
        0 => "North",
        1 => "South",
        2 => "East",
        3 => "West",
        _ => "Center"
    };
}
