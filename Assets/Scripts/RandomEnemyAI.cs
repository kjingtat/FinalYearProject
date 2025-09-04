using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomEnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;
    public int contactDamage = 10;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float timer = 0f;
    private Transform player;
    private EnemyFacing enemyFacing; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyFacing = GetComponent<EnemyFacing>(); 
        PickNewDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            PickNewDirection();
            timer = 0f;
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void PickNewDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;

        if (enemyFacing != null)
            enemyFacing.SetMoveDirection(moveDirection);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
            }
        }
    }
}
