using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemy = other.GetComponent<Health>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy arrow on hit
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Floor"))
        {
            Destroy(gameObject); // Hit wall or other object
        }
    }

}
