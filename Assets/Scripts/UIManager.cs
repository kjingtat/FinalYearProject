using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerUI playerUI;
    public PauseGame pauseGame;
    public DeathUI deathUI;
    public GameManager gameManager;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerUI = FindFirstObjectByType<PlayerUI>();
        deathUI = FindFirstObjectByType<DeathUI>();
        pauseGame = FindFirstObjectByType<PauseGame>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (playerUI != null)
        {
            playerUI.hpText = GameObject.Find("PlayerHP")?.GetComponent<TextMeshProUGUI>();
            playerUI.dmgText = GameObject.Find("PlayerDMG")?.GetComponent<TextMeshProUGUI>();
            playerUI.atkSpeedText = GameObject.Find("PlayerAtkSpd")?.GetComponent<TextMeshProUGUI>();
            playerUI.invulnText = GameObject.Find("PlayerInvuln")?.GetComponent<TextMeshProUGUI>();
        }

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null && playerUI != null)
        {
            Health playerHealth = playerObj.GetComponent<Health>();
            PlayerStats playerStats = playerObj.GetComponent<PlayerStats>();

            playerUI.Setup(playerHealth, playerStats);
        }

        if (deathUI != null)
        {
            deathUI.deathScreenUI = GameObject.Find("DeathScreen");
            if (deathUI.deathScreenUI != null)
                deathUI.deathScreenUI.SetActive(false);

            if (playerObj != null)
            {
                deathUI.playerHealth = playerObj.GetComponent<Health>();
                deathUI.HookDeathEvent();
            }

            deathUI.AssignButtons();
        }

        if (pauseGame != null)
        {
            pauseGame.gameUI = GameObject.Find("GameUI");
            pauseGame.menuUI = GameObject.Find("InGameMenuUI");

            if (pauseGame.menuUI != null)
                pauseGame.menuUI.SetActive(false);

            pauseGame.AssignButtons();
        }

        if (gameManager != null)
        {
            gameManager.winScreen = GameObject.Find("WinUI");
            if (gameManager.winScreen != null)
                gameManager.winScreen.SetActive(false);

            gameManager.AssignButtons();
        }

        RoomLayoutGenerator generator = FindFirstObjectByType<RoomLayoutGenerator>();
        if (generator != null)
        {
            generator.playerUI = playerUI;
            generator.deathUI = deathUI;
        }
    }
}
