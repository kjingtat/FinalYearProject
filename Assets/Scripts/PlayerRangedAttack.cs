using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject crosshairPrefab;
    private GameObject crosshair;

    public float arrowSpeed = 10f;
    public float spawnOffsetDistance = 0.5f;

    void Start()
    {
        // Spawn the crosshair
        crosshair = Instantiate(crosshairPrefab);
        crosshair.name = "Crosshair";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && crosshair != null)
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        Vector2 direction = ((Vector2)crosshair.transform.position - (Vector2)transform.position).normalized;
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnOffsetDistance;

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * arrowSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log($"Arrow shot toward {direction}, from {spawnPos}");
    }
}
