using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public GameObject mapPanel;        
    public GameObject mapBackground;    
    public GameObject roomIconPrefab;
    public GameObject clearedXPrefab;
    public GameObject playerMarkerPrefab;
    public float roomIconSpacing = 100f;

    private bool mapActive = false;
    private Dictionary<Vector2Int, RoomBehavior> rooms = new();
    private Dictionary<Vector2Int, GameObject> roomIcons = new();
    private Dictionary<Vector2Int, GameObject> clearedXIcons = new();
    private GameObject playerMarker;

    private void Awake()
    {
        Instance = this;

        if (mapPanel == null)
            mapPanel = GameObject.Find("MapUI");

        if (mapBackground == null && mapPanel != null)
            mapBackground = mapPanel.transform.Find("MapBackground")?.gameObject;

        mapPanel?.SetActive(false);

        if (playerMarkerPrefab != null && mapPanel != null)
        {
            playerMarker = Instantiate(playerMarkerPrefab, mapPanel.transform);
            playerMarker.SetActive(true);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapActive = !mapActive;
            mapPanel.SetActive(mapActive);
        }
    }

    public void InitializeMap(RoomBehavior[] allRooms)
    {
        rooms.Clear();
        roomIcons.Clear();
        clearedXIcons.Clear();

        foreach (var room in allRooms)
        {
            Vector2Int pos = room.gridPosition;
            rooms[pos] = room;

            GameObject iconGO = Instantiate(roomIconPrefab, mapPanel.transform);
            iconGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x * roomIconSpacing, pos.y * roomIconSpacing);
            iconGO.SetActive(room.isStartingRoom);

            Image iconImage = iconGO.GetComponent<Image>();
            iconImage.color = room.isStartingRoom ? Color.green : Color.gray;

            roomIcons[pos] = iconGO;

            GameObject xGO = Instantiate(clearedXPrefab, iconGO.transform);
            RectTransform rt = xGO.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            xGO.SetActive(false);
            clearedXIcons[pos] = xGO;

            DrawDoors(room, iconGO);
        }
    }

    public void VisitRoom(Vector2Int pos, bool cleared = false)
    {
        if (!roomIcons.ContainsKey(pos)) return;

        roomIcons[pos].SetActive(true);

        Image icon = roomIcons[pos].GetComponent<Image>();
        if (!rooms[pos].isStartingRoom)
            icon.color = cleared ? Color.white : Color.yellow;

        if (clearedXIcons.ContainsKey(pos))
            clearedXIcons[pos].SetActive(cleared);

        PlacePlayerMarker(pos);
        CenterOnPlayer(pos);
    }

    private void PlacePlayerMarker(Vector2Int pos)
    {
        if (playerMarker == null || !roomIcons.ContainsKey(pos)) return;

        RectTransform iconRT = roomIcons[pos].GetComponent<RectTransform>();
        RectTransform markerRT = playerMarker.GetComponent<RectTransform>();
        markerRT.position = iconRT.position;
        markerRT.SetAsLastSibling();
    }

    private void CenterOnPlayer(Vector2Int pos)
    {
        if (!roomIcons.ContainsKey(pos)) return;

        RectTransform iconRT = roomIcons[pos].GetComponent<RectTransform>();
        RectTransform panelRT = mapPanel.GetComponent<RectTransform>();
        Vector2 panelSize = panelRT.rect.size;

        Vector2 offset = -iconRT.anchoredPosition + panelSize / 2f;

        foreach (var kv in roomIcons)
        {
            RectTransform rt = kv.Value.GetComponent<RectTransform>();
            rt.anchoredPosition += offset;
        }

        if (playerMarker != null)
        {
            RectTransform markerRT = playerMarker.GetComponent<RectTransform>();
            markerRT.anchoredPosition += offset;
        }
    }

    private void DrawDoors(RoomBehavior room, GameObject parentIcon)
    {
        float doorLength = roomIconSpacing * 0.5f;
        float doorThickness = 2f;

        for (int i = 0; i < 4; i++)
        {
            if (!room.HasDoor(i)) continue;

            GameObject line = new GameObject("DoorLine", typeof(RectTransform));
            line.transform.SetParent(parentIcon.transform, false);
            Image img = line.AddComponent<Image>();
            img.color = Color.white;

            RectTransform rt = line.GetComponent<RectTransform>();
            if (i < 2)
            {
                rt.sizeDelta = new Vector2(doorThickness, doorLength);
                rt.anchoredPosition = new Vector2(0, i == 0 ? doorLength / 2 : -doorLength / 2);
            }
            else
            {
                rt.sizeDelta = new Vector2(doorLength, doorThickness);
                rt.anchoredPosition = new Vector2(i == 2 ? doorLength / 2 : -doorLength / 2, 0);
            }
        }
    }
}
