using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 5f; 
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float attackDamage = 1f;
    public float fireRate = 10f;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.SetMaxHealth(maxHealth);
        }
        moveSpeed = baseMoveSpeed; 
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        if (health != null && CompareTag("Player"))
        {
            health.SetMaxHealth(maxHealth, healByIncrease: true);
        }
    }

    public void IncreaseMoveSpeed(float amount)
    {
        baseMoveSpeed += amount; 
        moveSpeed = baseMoveSpeed;
    }

    public void IncreaseAttackDamage(float amount)
    {
        attackDamage += amount;
    }

    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;
    }

    public void IncreaseInvulnerabilityTime(float amount)
    {
        if (health != null)
        {
            health.invulnerabilityDuration += amount;
        }
    }
}
