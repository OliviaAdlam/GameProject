using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveHorizontal { get; private set; }
    public float moveVertical { get; private set; }

    private Vector2 lastMovementDirection;
    public GameObject sword;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        lastMovementDirection = Vector2.zero;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * moveSpeed * Time.deltaTime;

        if (movement.magnitude > 0)
        {
            lastMovementDirection = movement.normalized;
        }

        UpdateAnimations(movement);
        PositionSword();
    }

    void UpdateAnimations(Vector3 movement)
    {
        animator.SetBool("isMoving", movement.magnitude > 0);

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isAttacking", false);
        }
    }

    void PositionSword()
    {
        if (sword == null) return;

        Vector3 swordPosition = transform.position;

        if (lastMovementDirection != Vector2.zero)
        {
            if (lastMovementDirection.x < 0)
            {
                swordPosition.x = transform.position.x - 2f;
            }
            else if (lastMovementDirection.x > 0)
            {
                swordPosition.x = transform.position.x + 2f;
            }

            if (lastMovementDirection.y < 0)
            {
                swordPosition.y = transform.position.y - 2f;
            }
            else if (lastMovementDirection.y > 0)
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