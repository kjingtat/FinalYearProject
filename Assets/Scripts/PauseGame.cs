using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameUI;   
    public GameObject menuUI;   

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        menuUI.SetActive(true);
        gameUI.SetActive(false);

        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        menuUI.SetActive(false);
        gameUI.SetActive(true);

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void AssignButtons()
    {
        if (menuUI == null)
            return;

        Transform resumeTransform = menuUI.transform.Find("ResumeButton");
        if (resumeTransform != null)
        {
            Button resumeBtn = resumeTransform.GetComponent<Button>();
            resumeBtn.onClick.RemoveAllListeners();
            resumeBtn.onClick.AddListener(Resume);
        }

        Transform menuTransform = menuUI.transform.Find("MainMenuButton");
        if (menuTransform != null)
        {
            Button menuBtn = menuTransform.GetComponent<Button>();
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(LoadMainMenu);
        }
    }

}
