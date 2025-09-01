using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PlayerStats playerStats;  
    private Health playerHealth;     

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            if (food != null && playerHealth != null)
            {
                food.Consume(playerHealth);
            }
        }
        else if (other.CompareTag("Upgrade"))
        {
            MushUpgrade upgrade = other.GetComponent<MushUpgrade>();
            if (upgrade != null && playerStats != null)
            {
                ApplyUpgrade(upgrade);
                Destroy(other.gameObject);
            }
        }
    }

    private void ApplyUpgrade(MushUpgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Health:
                playerStats.IncreaseMaxHealth(upgrade.amount);
                break;
            case UpgradeType.Damage:
                playerStats.IncreaseAttackDamage(upgrade.amount);
                break;
            case UpgradeType.FireRate:
                playerStats.IncreaseFireRate(upgrade.amount);
                break;
            case UpgradeType.Invulnerability:
                playerStats.IncreaseInvulnerabilityTime(upgrade.amount);
                break;
        }

    }
}
