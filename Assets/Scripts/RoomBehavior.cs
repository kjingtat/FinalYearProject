using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoomBehavior : MonoBehaviour
{
    [Header("Walls and Doors")]
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] locked;

    [Header("Player Spawn Points")]
    public Transform spawnPointCenter;
    public Transform spawnPointNorth;
    public Transform spawnPointEast;
    public Transform spawnPointSouth;
    public Transform spawnPointWest;

    [Header("Enemy Spawning")]
    public GameObject[] tier1Enemies;
    public GameObject[] tier2Enemies;
    public GameObject[] tier3Enemies;
    public GameObject[] enemySpawnPoints;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool hasSpawnedEnemies = false;

    [Header("Room Settings")]
    public bool isStartingRoom = false;
    public bool isCleared = false;

    private bool[] doorStatus = new bool[4];

    [Header("Tier Unlock Settings")]
    public int roomsToUnlockTier2 = 6;
    public int roomsToUnlockTier3 = 12;

    [Header("Spawn Count Settings")]
    public Vector2Int tier1SpawnRange = new Vector2Int(3, 5);
    public Vector2Int tier2SpawnRange = new Vector2Int(5, 7);
    public Vector2Int tier3SpawnRange = new Vector2Int(8, 10);

    [Header("Grid Info")]
    public Vector2Int gridPosition;

    public void UpdateRoom(bool[] status)
    {
        doorStatus = status;

        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    public Vector2 GetSpawnPosition(string direction)
    {
        return direction switch
        {
            "North" => spawnPointNorth.position,
            "East" => spawnPointEast.position,
            "South" => spawnPointSouth.position,
            "West" => spawnPointWest.position,
            _ => spawnPointCenter.position,
        };
    }

    public void OnPlayerEnterRoom()
    {
        if (isStartingRoom || isCleared) return;

        MapManager.Instance.VisitRoom(gridPosition, cleared: false);

        if (!hasSpawnedEnemies)
        {
            SpawnEnemies();
            hasSpawnedEnemies = true;

            for (int i = 0; i < doors.Length; i++)
            {
                if (doorStatus[i])
                {
                    doors[i].SetActive(false);
                    locked[i].SetActive(true);
                }
            }
        }
    }

    public bool HasDoor(int index)
    {
        if (index < 0 || index >= doorStatus.Length) return false;
        return doorStatus[index];
    }

    private void SpawnEnemies()
    {
        spawnedEnemies.Clear();

        int clearedRooms = GameManager.Instance != null ? GameManager.Instance.RoomsCleared : 0;

        List<GameObject> availableEnemies = new List<GameObject>(tier1Enemies);

        if (clearedRooms >= roomsToUnlockTier2)
            availableEnemies.AddRange(tier2Enemies);

        if (clearedRooms >= roomsToUnlockTier3)
            availableEnemies.AddRange(tier3Enemies);

        Vector2Int spawnRange = tier1SpawnRange;
        if (clearedRooms >= roomsToUnlockTier2) spawnRange = tier2SpawnRange;
        if (clearedRooms >= roomsToUnlockTier3) spawnRange = tier3SpawnRange;

        int spawnCount = Random.Range(spawnRange.x, spawnRange.y + 1);
        spawnCount = Mathf.Min(spawnCount, enemySpawnPoints.Length); 

        List<int> availableIndices = Enumerable.Range(0, enemySpawnPoints.Length).ToList();

        for (int i = 0; i < spawnCount; i++)
        {
            if (availableIndices.Count == 0 || availableEnemies.Count == 0) break;

            int randomSpawnIndex = Random.Range(0, availableIndices.Count);
            int spawnPointIndex = availableIndices[randomSpawnIndex];
            availableIndices.RemoveAt(randomSpawnIndex);

            GameObject enemyPrefab = availableEnemies[Random.Range(0, availableEnemies.Count)];

            GameObject newEnemy = Instantiate(
                enemyPrefab,
                enemySpawnPoints[spawnPointIndex].transform.position,
                Quaternion.identity
            );

            spawnedEnemies.Add(newEnemy);
        }
    }


    private GameObject GetEnemyPrefabByTier(int tier)
    {
        switch (tier)
        {
            case 1:
                return tier1Enemies.Length > 0 ? tier1Enemies[Random.Range(0, tier1Enemies.Length)] : null;
            case 2:
                return tier2Enemies.Length > 0 ? tier2Enemies[Random.Range(0, tier2Enemies.Length)] : null;
            case 3:
                return tier3Enemies.Length > 0 ? tier3Enemies[Random.Range(0, tier3Enemies.Length)] : null;
            default:
                return null;
        }
    }

    private void Update()
    {
        if (hasSpawnedEnemies && spawnedEnemies.Count > 0)
        {
            spawnedEnemies.RemoveAll(enemy => enemy == null);

            if (spawnedEnemies.Count == 0)
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    if (doorStatus[i])
                    {
                        doors[i].SetActive(true);
                        locked[i].SetActive(false);
                    }
                }

                hasSpawnedEnemies = false;
                isCleared = true;

                MapManager.Instance.VisitRoom(gridPosition, cleared: true);

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RoomCleared();

                    if (!GameManager.Instance.HasWon)
                    {
                        StatsUpgradeManager upgradeManager = FindFirstObjectByType<StatsUpgradeManager>();
                        if (upgradeManager != null)
                        {
                            upgradeManager.ShowUpgradeOptions();
                        }
                    }
                }
            }
        }
    }
}
