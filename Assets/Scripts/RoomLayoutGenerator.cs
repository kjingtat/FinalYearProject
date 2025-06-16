using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGenerator : MonoBehaviour
{
    public PlayerUI playerUI;

    public GameObject playerPrefab;
    private GameObject playerInstance;
    private GameObject firstRoomInstance;

    [Header("Dungeon Generation Settings")]
    public int maxRooms = 10;
    public GameObject roomPrefab;

    [Header("Room Grid Spacing")]
    public float roomSpacingX = 18f; // Adjust based on 17 tile width
    public float roomSpacingY = 11f; // Adjust based on 10 tile height

    [Header("Position Offset")]
    public Vector2 startRoomWorldPosition = new Vector2(0.3f, -0.1f);

    private HashSet<Vector2Int> placedRooms = new();
    private Vector2Int startRoom = Vector2Int.zero;

    void Start()
    {
        GenerateRooms();
        SpawnPlayer();
    }

    void GenerateRooms()
    {
        Vector2Int currentPos = Vector2Int.zero;
        startRoom = currentPos;
        placedRooms.Add(currentPos);

        for (int i = 1; i < maxRooms; i++)
        {
            Vector2Int newPos = GetNewPosition(currentPos);

            int tries = 0;
            while (placedRooms.Contains(newPos) && tries < 10)
            {
                newPos = GetNewPosition(currentPos);
                tries++;
            }

            if (!placedRooms.Contains(newPos))
            {
                placedRooms.Add(newPos);
                currentPos = newPos;
            }
        }

        InstantiateRooms();
    }

    Vector2Int GetNewPosition(Vector2Int pos)
    {
        Vector2Int[] directions = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        return pos + directions[Random.Range(0, directions.Length)];
    }

    void InstantiateRooms()
    {
        foreach (var pos in placedRooms)
        {
            Vector3 worldPos = new Vector3(pos.x * roomSpacingX, pos.y * roomSpacingY, 0);
            worldPos += (Vector3)startRoomWorldPosition;

            GameObject room = Instantiate(roomPrefab, worldPos, Quaternion.identity, this.transform);

            bool[] doorStatus = new bool[4];
            doorStatus[0] = placedRooms.Contains(pos + Vector2Int.up);    // Spawn room Up
            doorStatus[1] = placedRooms.Contains(pos + Vector2Int.down);  // Spawn room Down
            doorStatus[2] = placedRooms.Contains(pos + Vector2Int.right); // Spawn room Right
            doorStatus[3] = placedRooms.Contains(pos + Vector2Int.left);  // Spawn room Left

            RoomBehavior rb = room.GetComponent<RoomBehavior>();
            if (rb != null)
            {
                rb.UpdateRoom(doorStatus);

                if (pos == Vector2Int.zero)
                {
                    rb.isStartingRoom = true;
                    firstRoomInstance = room;
                }
            }
        }
    }

    void SpawnPlayer()
    {
        if (firstRoomInstance == null)
        {
            Debug.LogError("firstRoomInstance is null — room was not instantiated");
            return;
        }

        var roomBehavior = firstRoomInstance.GetComponent<RoomBehavior>();
        if (roomBehavior == null)
        {
            Debug.LogError("RoomBehavior component missing on firstRoomInstance");
            return;
        }

        if (roomBehavior.spawnPointCenter == null)
        {
            Debug.LogError("spawnPointCenter is not assigned on RoomBehavior");
            return;
        }

        Vector2 spawnPosition = roomBehavior.GetSpawnPosition("Center");

        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        RoomManager.Instance.player = playerInstance;
        RoomManager.Instance.InitializeRoomMap(GetComponentsInChildren<RoomBehavior>());

        playerUI.playerHealth = playerInstance.GetComponent<Health>();

    }
}
