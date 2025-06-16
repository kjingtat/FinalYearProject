using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    [Header("Walls and Doors")]
    public GameObject[] walls; //0 - up, 1 - down, 2 - right, 3 - left
    public GameObject[] doors;

    [Header("Player Spawn Points")]
    public Transform spawnPointCenter;
    public Transform spawnPointNorth;
    public Transform spawnPointEast;
    public Transform spawnPointSouth;
    public Transform spawnPointWest;

    [Header("Enemy Spawning")]
    public GameObject[] enemyPrefabs; // Assign in Inspector
    public GameObject[] enemySpawnPoints; // Assign 4 spawn points in Inspector
    private bool hasSpawnedEnemies = false;

    [Header("Room Settings")]
    public bool isStartingRoom = false;

    public void UpdateRoom(bool[] status)
    {
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
        if (isStartingRoom) return;  // Skip spawning enemies in the starting room

        if (!hasSpawnedEnemies)
        {
            SpawnEnemies();
            hasSpawnedEnemies = true;
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomIndex], enemySpawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}
