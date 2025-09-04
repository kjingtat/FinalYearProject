using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private RoomBehavior[] allRooms;
    public GameObject winScreen;

    public bool HasWon { get; private set; } = false;
    public int RoomsCleared { get; private set; } = 0;

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
            return;
        }
    }

    public void RegisterRooms(RoomBehavior[] rooms)
    {
        allRooms = rooms;
    }

    public void RoomCleared()
    {
        RoomsCleared++;
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (allRooms == null || allRooms.Length == 0)
            return;

        foreach (var room in allRooms)
        {
            if (!room.isStartingRoom && !room.isCleared)
                return;
        }

        HasWon = true;

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void ResetRunState()
    {
        RoomsCleared = 0;
        HasWon = false;
        allRooms = null;

        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void RestartRun()
    {
        ResetRunState();

        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void AssignButtons()
    {
        if (winScreen == null)
            return;

        Transform restartTransform = winScreen.transform.Find("RestartButton");
        if (restartTransform != null)
        {
            Button restartBtn = restartTransform.GetComponent<Button>();
            restartBtn.onClick.RemoveAllListeners();
            restartBtn.onClick.AddListener(RestartRun);
        }

        Transform menuTransform = winScreen.transform.Find("MainMenuButton");
        if (menuTransform != null)
        {
            Button menuBtn = menuTransform.GetComponent<Button>();
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(LoadMainMenu);
        }
    }
}
