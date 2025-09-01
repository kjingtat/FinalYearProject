using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;

    private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        FlipToMouse();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * playerStats.moveSpeed;
    }

    void FlipToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); 
        else
            transform.localScale = new Vector3(1, 1, 1);  
    }
}
