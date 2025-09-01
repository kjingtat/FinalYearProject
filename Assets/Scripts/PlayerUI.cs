using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI atkSpeedText;
    public TextMeshProUGUI invulnText;

    public Health playerHealth;
    public PlayerStats playerStats;

    private float lastDamageValue;
    private float lastAttackSpeedValue;
    private float lastInvulnValue;

    public void Setup(Health newHealth, PlayerStats newStats)
    {
        playerHealth = newHealth;
        playerStats = newStats;

        if (playerHealth != null)
        {
            hpText.text = ": " + playerHealth.CurrentHP;
            lastInvulnValue = playerHealth.invulnerabilityDuration;
            invulnText.text = ": " + lastInvulnValue.ToString("0.00") + "s";
        }

        if (playerStats != null)
        {
            lastDamageValue = playerStats.attackDamage;
            dmgText.text = ": " + lastDamageValue;

            lastAttackSpeedValue = playerStats.fireRate;
            atkSpeedText.text = ": " + lastAttackSpeedValue.ToString("0.0") + " /s";
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            hpText.text = ": " + playerHealth.CurrentHP;
        }
        else
        {
            hpText.text = ": ?";
        }

        if (playerStats != null && playerStats.attackDamage != lastDamageValue)
        {
            lastDamageValue = playerStats.attackDamage;
            dmgText.text = ": " + lastDamageValue;
        }

        if (playerStats != null && playerStats.fireRate != lastAttackSpeedValue)
        {
            lastAttackSpeedValue = playerStats.fireRate;
            atkSpeedText.text = ": " + lastAttackSpeedValue.ToString("0.0") + " /s";
        }

        if (playerHealth != null && playerHealth.invulnerabilityDuration != lastInvulnValue)
        {
            lastInvulnValue = playerHealth.invulnerabilityDuration;
            invulnText.text = ": " + lastInvulnValue.ToString("0.00") + "s";
        }
    }
}
