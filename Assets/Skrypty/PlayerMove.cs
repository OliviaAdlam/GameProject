using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }

    private Vector2 lastMovementDirection; // Last movement direction
    public GameObject sword; // Reference to sword object

    void Start()
    {
        lastMovementDirection = Vector2.zero; // No movement at the start
    }

    void Update()
    {
        // Get input for movement (arrow keys or WASD)
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        // Update player's position
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Update last movement direction
        if (movement.magnitude > 0)
        {
            lastMovementDirection = movement.normalized;
        }

        // Update sword position based on movement direction
        PositionSword();
    }

    void PositionSword()
    {
        if (sword == null) return;

        Vector3 swordPosition = transform.position;

        if (lastMovementDirection != Vector2.zero)
        {
            if (lastMovementDirection.x < 0) // Move left
            {
                swordPosition.x = transform.position.x - 2f;
            }
            else if (lastMovementDirection.x > 0) // Move right
            {
                swordPosition.x = transform.position.x + 2f;
            }

            if (lastMovementDirection.y < 0) // Move down
            {
                swordPosition.y = transform.position.y - 2f;
            }
            else if (lastMovementDirection.y > 0) // Move up
            {
                swordPosition.y = transform.position.y + 2f;
            }
        }

        sword.transform.position = swordPosition;
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }
}
