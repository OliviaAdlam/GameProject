using UnityEngine;

public class Smol_Enemy : Enemy
{
    public float speed = 1.5f;
    public float detectionRange = 5.0f;
    public int contactDamage = 1;
    public float randomMoveDuration = 2.0f;
    public float changeDirectionCooldown = 1.0f;

    private Transform player;
    private Vector2 randomDirection;
    private float lastDirectionChangeTime;
    private float lastRandomMoveTime;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the object!");
        }

        ChangeRandomDirection();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
        }
        else
        {
            MoveRandomly();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Obracanie sprite'a w stronę gracza
        FlipSprite(direction.x);

        transform.position += direction * speed * Time.deltaTime;
    }

    void MoveRandomly()
    {
        if (Time.time - lastRandomMoveTime >= randomMoveDuration)
        {
            ChangeRandomDirection();
            lastRandomMoveTime = Time.time;
        }

        // Obracanie sprite'a w kierunku ruchu
        FlipSprite(randomDirection.x);

        transform.position += (Vector3)randomDirection * speed * Time.deltaTime;
    }

    public void ChangeRandomDirection()
    {
        if (Time.time - lastDirectionChangeTime >= changeDirectionCooldown)
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            randomDirection = new Vector2(randomX, randomY).normalized;
            lastDirectionChangeTime = Time.time;
        }
    }

    void FlipSprite(float directionX)
    {
        if (directionX > 0)
        {
            spriteRenderer.flipX = false; // Patrzy w lewo
        }
        else if (directionX < 0)
        {
            spriteRenderer.flipX = true; // Patrzy w prawo
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(contactDamage);
            }
        }
    }
}
