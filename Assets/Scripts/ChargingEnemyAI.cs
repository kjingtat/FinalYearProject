using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ChargingEnemyAI : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float detectionRange = 6f;
    public float chargeSpeed = 8f;
    public float chargeCooldown = 2f;
    public int contactDamage = 15;

    private Rigidbody2D rb;
    private Collider2D col;
    private Transform player;
    private bool isCharging = false;
    private Vector2 chargeDirection;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Collider2D[] allEnemies = GameObject.FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (Collider2D other in allEnemies)
        {
            if (other != col && other.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(col, other, true);
            }
        }
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (!isCharging)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && cooldownTimer <= 0f)
            {
                StartCharge();
            }
        }
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            Vector2 newPosition = rb.position + chargeDirection * chargeSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
        else
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        chargeDirection = ((Vector2)player.position - rb.position).normalized;
    }

    private void StopCharge()
    {
        isCharging = false;
        cooldownTimer = chargeCooldown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(contactDamage);
        }

        if (!collision.collider.CompareTag("Enemy"))
        {
            StopCharge();
        }
    }
}
