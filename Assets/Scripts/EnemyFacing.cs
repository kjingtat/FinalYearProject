using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    private Transform player;
    public bool isRandom = false; 
    public GameObject healthBarCanvas; 

    private Vector2 lastMoveDirection;

    public void SetTarget(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetMoveDirection(Vector2 moveDir)
    {
        lastMoveDirection = moveDir;
    }

    void Update()
    {
        if (isRandom)
            FlipByMovement();
        else if (player != null)
            FlipToPlayer();
    }

    void FlipToPlayer()
    {
        if (player.position.x < transform.position.x)
            SetFlip(-1);
        else
            SetFlip(1);
    }

    void FlipByMovement()
    {
        if (lastMoveDirection.x < 0)
            SetFlip(-1);
        else if (lastMoveDirection.x > 0)
            SetFlip(1);
    }

    void SetFlip(float xScale)
    {
        Vector3 newScale = new Vector3(xScale, 1, 1);
        transform.localScale = newScale;

        if (healthBarCanvas != null)
        {
            Vector3 healthBarScale = healthBarCanvas.transform.localScale;
            healthBarCanvas.transform.localScale = new Vector3(
                Mathf.Abs(healthBarScale.x) * (xScale > 0 ? 1 : -1),
                healthBarScale.y,
                healthBarScale.z
            );
        }
    }
}
