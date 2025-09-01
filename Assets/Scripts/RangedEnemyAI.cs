using UnityEngine;
using System.Collections;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float randomMoveInterval = 1.5f;
    public float randomMoveRadius = 2f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public float projectileSpeed = 7f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 randomOffset;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(RandomMoveRoutine());
    }

    void Update()
    {
        if (player == null) return;

        rb.linearVelocity = randomOffset * moveSpeed;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ShootProjectile();
            lastAttackTime = Time.time;
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 shootDir = (player.position - firePoint.position).normalized;
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.linearVelocity = shootDir * projectileSpeed;
        }
    }

    private IEnumerator RandomMoveRoutine()
    {
        while (true)
        {
            randomOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * randomMoveRadius;
            yield return new WaitForSeconds(randomMoveInterval);
        }
    }
}
