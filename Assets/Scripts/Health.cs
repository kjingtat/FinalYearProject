using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    public float invulnerabilityDuration = 1.5f;
    private bool isInvulnerable = false;

    private SpriteRenderer spriteRenderer;

    public delegate void OnDeath();
    public event OnDeath onDeath;

    public event Action<int, int> OnHealthChanged;

    [Header("Enemy Drop Settings")]
    public float dropChance = 0.1f;
    public GameObject[] possibleDrops; 

    void Awake()
    {
        if (CompareTag("Player"))
        {
            PlayerStats stats = GetComponent<PlayerStats>();
            if (stats != null)
            {
                maxHealth = Mathf.RoundToInt(stats.maxHealth);
            }
        }

        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount, bool ignoreInvuln = false)
    {
        if (CompareTag("Player") && isInvulnerable && !ignoreInvuln)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (CompareTag("Player") && !ignoreInvuln)
        {
            StartCoroutine(HandleInvulnerability());
        }

        if (currentHealth <= 0)
        {
            if (CompareTag("Enemy"))
            {
                HandleEnemyDrop();
                Destroy(gameObject);
            }
            else if (CompareTag("Player"))
            {
                onDeath?.Invoke();
            }
        }
    }


    private void HandleEnemyDrop()
    {
        if (possibleDrops.Length == 0) return;

        if (UnityEngine.Random.value <= dropChance)
        {
            GameObject itemToDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    private System.Collections.IEnumerator HandleInvulnerability()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        float blinkInterval = 0.2f;
        Color originalColor = spriteRenderer.color;
        Color blinkColor = Color.gray;

        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.color = originalColor;
        isInvulnerable = false;
    }

    public void SetMaxHealth(float newMax, bool healByIncrease = false)
    {
        int oldMax = maxHealth;
        maxHealth = Mathf.RoundToInt(newMax);

        if (healByIncrease && maxHealth > oldMax)
        {
            Heal(maxHealth - oldMax);
        }
        else
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public int CurrentHP => currentHealth;
    public int MaxHP => maxHealth;
}
