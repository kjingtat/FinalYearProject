using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button[] upgradeButtons;
    public StatsUpgradeOptions[] allUpgrades; 

    public PlayerStats playerStats;

    private void Start()
    {
        upgradePanel.SetActive(false); 
    }

    public void ShowUpgradeOptions()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            StatsUpgradeOptions option = allUpgrades[Random.Range(0, allUpgrades.Length)];

            int capturedIndex = i; 

            TextMeshProUGUI buttonText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = option.name + "\n" + option.description;

            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(option));
        }
    }

    public void ApplyUpgrade(StatsUpgradeOptions option)
    {
        switch (option.stat)
        {
            case StatsUpgradeOptions.StatType.MaxHealth:
                playerStats.IncreaseMaxHealth(option.amount);
                playerStats.GetComponent<Health>().SetMaxHealth(playerStats.maxHealth, healByIncrease: true);
                break;
            case StatsUpgradeOptions.StatType.MoveSpeed:
                playerStats.IncreaseMoveSpeed(option.amount);
                break;
            case StatsUpgradeOptions.StatType.AttackDamage:
                playerStats.IncreaseAttackDamage(option.amount);
                break;
            case StatsUpgradeOptions.StatType.FireRate:
                playerStats.IncreaseFireRate(option.amount);
                break;
            case StatsUpgradeOptions.StatType.InvulnerabilityTime:
                playerStats.IncreaseInvulnerabilityTime(option.amount);
                break;
        }

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
