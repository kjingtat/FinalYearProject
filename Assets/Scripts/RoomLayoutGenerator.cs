using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGenerator : MonoBehaviour
{
    public PlayerUI playerUI;
    public DeathUI deathUI;

    public GameObject playerPrefab;
    private GameObject playerInstance;
    private GameObject firstRoomInstance;

    [Header("Dungeon Generation Settings")]
    public int maxRooms = 10;
    public GameObject roomPrefab;

    [Header("Room Grid Spacing")]
    public float roomSpacingX = 18f; 
    public float roomSpacingY = 11f; 

    [Header("Position Offset")]
    public Vector2 startRoomWorldPosition = new Vector2(0.3f, -0.1f);

    private HashSet<Vector2Int> placedRooms = new();
    private Vector2Int startRoom = Vector2Int.zero;

    void Start()
    {
        GenerateRooms();
        StartCoroutine(SpawnPlayerNextFrame());
    }

    private IEnumerator SpawnPlayerNextFrame()
    {
        yield return null;
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
            doorStatus[0] = placedRooms.Contains(pos + Vector2Int.up); 
            doorStatus[1] = placedRooms.Contains(pos + Vector2Int.down);  
            doorStatus[2] = placedRooms.Contains(pos + Vector2Int.right);
            doorStatus[3] = placedRooms.Contains(pos + Vector2Int.left);  

            RoomBehavior rb = room.GetComponent<RoomBehavior>();
            if (rb != null)
            {
                rb.gridPosition = pos;     
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
            return;

        var roomBehavior = firstRoomInstance.GetComponent<RoomBehavior>();
        if (roomBehavior == null || roomBehavior.spawnPointCenter == null)
            return;

        Vector2 spawnPosition = roomBehavior.GetSpawnPosition("Center");
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        var playerStats = playerInstance.GetComponent<PlayerStats>();
        var playerHealth = playerInstance.GetComponent<Health>();

        var statsManager = FindAnyObjectByType<StatsUpgradeManager>();
        if (statsManager != null)
            statsManager.playerStats = playerStats;

        RoomManager.Instance.player = playerInstance;
        RoomManager.Instance.InitializeRoomMap(GetComponentsInChildren<RoomBehavior>());

        if (playerUI != null)
            playerUI.Setup(playerHealth, playerStats);

        if (deathUI != null)
        {
            deathUI.playerHealth = playerHealth;
            deathUI.HookDeathEvent();
        }

        RoomBehavior[] rooms = GetComponentsInChildren<RoomBehavior>();

        MapManager.Instance.InitializeMap(rooms);
        MapManager.Instance.VisitRoom(Vector2Int.zero, cleared: false); 

        GameManager.Instance.RegisterRooms(rooms);
    }

}
