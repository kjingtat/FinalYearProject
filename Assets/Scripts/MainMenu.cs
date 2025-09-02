using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject InstructionUI; 

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInstructions()
    {
        if (InstructionUI != null)
        {
            InstructionUI.SetActive(true);
        }
    }

    public void HideInstructions()
    {
        if (InstructionUI != null)
        {
            InstructionUI.SetActive(false);
        }
    }
}
