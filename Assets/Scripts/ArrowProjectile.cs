using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private int damage;
    public float lifetime = 3f;

    private PlayerStats playerStats;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemy = other.GetComponent<Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (playerStats != null && playerStats.flatLifesteal > 0f)
                {
                    Health playerHealth = playerStats.GetComponent<Health>();
                    if (playerHealth != null)
                        playerHealth.Heal((int)playerStats.flatLifesteal); 
                }

                EnemyAI enemyAI = other.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.OnHitByPlayer();
                }
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Destruct"))
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }

}
