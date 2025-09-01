using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private int damage;
    public float lifetime = 3f;

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

}
