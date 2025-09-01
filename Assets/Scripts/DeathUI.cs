using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject deathScreenUI;      
    public Health playerHealth;

    void ShowDeathScreen()
    {
        Time.timeScale = 0f; 
        deathScreenUI.SetActive(true);

        Cursor.visible = true;

    }

    public void HookDeathEvent()
    {
        if (playerHealth != null)
        {
            playerHealth.onDeath += ShowDeathScreen;
        }
    }

    public void RestartRun()
    {
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
        if (deathScreenUI == null)
            return;

        Transform restartTransform = deathScreenUI.transform.Find("RestartButton");
        if (restartTransform != null)
        {
            Button restartBtn = restartTransform.GetComponent<Button>();
            restartBtn.onClick.RemoveAllListeners();
            restartBtn.onClick.AddListener(RestartRun);
        }

        Transform menuTransform = deathScreenUI.transform.Find("MainMenuButton");
        if (menuTransform != null)
        {
            Button menuBtn = menuTransform.GetComponent<Button>();
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(LoadMainMenu);
        }
    }

}
