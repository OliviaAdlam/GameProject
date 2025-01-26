using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int maxLives = 10;
    private int currentLives;
    private int playerScore = 0;
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public GameObject sword;

    private PlayerMove playerMove;

    void Start()
    {
        currentLives = 3;
        sword.SetActive(false);
        playerMove = GetComponent<PlayerMove>();
        ApplyUpgrades();
        
    }

    void Update()
    {
        UpdateSwordPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PerformAttack());
        }
    }

    public void AddCoins(int amount)
    {
        playerScore += amount;
        Debug.Log("Coins added: " + amount + ". Total: " + playerScore);
    }

    public void ApplyUpgrades()
    {
        maxLives = 10 + GlobalData.addLives;
        attackDamage = 1 + GlobalData.addAttack + (int)GlobalData.currentWeaponDamage;
        playerMove.moveSpeed = 5f + GlobalData.addSpeed;

        if (currentLives < maxLives)
        {
            currentLives = maxLives;
        }

        Debug.Log($"Aktualne statystyki: MaxLives={maxLives}, Attack={attackDamage}, Speed={playerMove.moveSpeed}");
    }

    IEnumerator PerformAttack()
    {
        if (sword == null || attackPoint == null)
        {
            yield break;
        }
        
        sword.SetActive(true);

        // Get the weapon component and its sprite renderer
        Weapon weaponComponent = sword.GetComponent<Weapon>();
        SpriteRenderer weaponSprite = sword.GetComponent<SpriteRenderer>();
        
        if (weaponSprite != null)
        {
            // Create a smaller hitbox based on the weapon's size
            Vector2 weaponSize = weaponSprite.bounds.size;
            Vector2 hitboxSize = new Vector2(weaponSize.x * 0.5f, weaponSize.y * 0.5f);
            
            // Calculate the attack point position (at the tip of the weapon)
            Vector2 attackPosition = attackPoint.position;
            
            // Get all colliders within the weapon's actual area
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(
                attackPosition,  // Use attack point position
                hitboxSize,     // Use smaller hitbox
                sword.transform.rotation.eulerAngles.z,
                enemyLayers
            );

            // Debug visualization
            Debug.DrawLine(sword.transform.position, attackPoint.position, Color.red, 0.5f);

            foreach (Collider2D hitCollider in hitColliders)
            {
                Enemy enemyScript = hitCollider.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Calculate distance from attack point to enemy
                    float distanceToEnemy = Vector2.Distance(attackPosition, hitCollider.transform.position);
                    
                    // Only damage enemies that are very close to the attack point
                    if (distanceToEnemy <= hitboxSize.magnitude)
                    {
                        Debug.Log($"Hit enemy at position {hitCollider.transform.position}, distance: {distanceToEnemy}");
                        enemyScript.TakeDamage(attackDamage);
                        break;
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        sword.SetActive(false);
    }

    void UpdateSwordPosition()
    {
        if (sword == null || playerMove == null || playerMove.sword == null) return;

        Vector3 swordPosition = playerMove.transform.position;

        // Only update if the sword reference is still valid
        if (playerMove.sword != null)
        {
            sword.transform.position = playerMove.sword.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only collect coins if it's the player colliding, not the weapon
        if (other.CompareTag("Coin") && other.gameObject.layer != LayerMask.NameToLayer("Weapon"))
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null)
            {
                GlobalData.totalCoins++;
                Destroy(other.gameObject);
            }
        }

        // Only take damage if the colliding object is an enemy AND we're the player (not the weapon)
        /*if (other.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.damage);
                Debug.Log("Took damage!");
            }
        }*/
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("Lives remaining: " + currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("Player died!");
            GameManager.Instance.PlayerDied();
            Destroy(gameObject);
        }
    }
}
