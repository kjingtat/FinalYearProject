using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

        List<StatsUpgradeOptions> shuffledUpgrades = new List<StatsUpgradeOptions>(allUpgrades);

        for (int i = 0; i < shuffledUpgrades.Count; i++)
        {
            StatsUpgradeOptions temp = shuffledUpgrades[i];
            int randomIndex = Random.Range(i, shuffledUpgrades.Count);
            shuffledUpgrades[i] = shuffledUpgrades[randomIndex];
            shuffledUpgrades[randomIndex] = temp;
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= shuffledUpgrades.Count) break; 

            StatsUpgradeOptions option = shuffledUpgrades[i];

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
            case StatsUpgradeOptions.StatType.Lifesteal:
                playerStats.IncreaseFlatLifesteal(option.amount);
                break;
        }

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
