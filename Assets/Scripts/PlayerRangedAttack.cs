using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject crosshairPrefab;
    public Transform firePoint; 
    private GameObject crosshair;

    public float arrowSpeed = 10f;

    private float cooldownTimer = 0f;

    private PlayerStats playerStats;

    public AudioClip shootSound;
    private AudioSource audioSource;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        crosshair = Instantiate(crosshairPrefab);
        crosshair.name = "Crosshair";

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && crosshair != null && cooldownTimer <= 0f)
        {
            ShootArrow();
            cooldownTimer = 1f / playerStats.fireRate;

            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    void ShootArrow()
    {
        Vector2 direction = ((Vector2)crosshair.transform.position - (Vector2)firePoint.position).normalized;
        Vector2 spawnPos = firePoint.position;

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * arrowSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        ArrowProjectile arrowScript = arrow.GetComponent<ArrowProjectile>();
        if (arrowScript != null)
        {
            arrowScript.SetDamage(Mathf.RoundToInt(playerStats.attackDamage));
            arrowScript.SetPlayerStats(playerStats); 
        }
    }

}
