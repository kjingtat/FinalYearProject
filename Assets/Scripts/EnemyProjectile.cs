using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 5; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Destruct"))
        {
            Destroy(gameObject);
        }
    }
}
