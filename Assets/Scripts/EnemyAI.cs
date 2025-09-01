using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public int contactDamage = 10;

    private Transform player;
    private Rigidbody2D rb;
    private bool hasDetectedPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!hasDetectedPlayer && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            hasDetectedPlayer = true;
        }
    }

    void FixedUpdate()
    {
        if (hasDetectedPlayer)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
                hasDetectedPlayer = true;
            }
        }
    }

    public void OnHitByPlayer()
    {
        hasDetectedPlayer = true;
    }
}
