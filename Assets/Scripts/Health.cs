using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public int CurrentHP => currentHealth; // Public read-only property
}
