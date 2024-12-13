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
        attackDamage = 1 + GlobalData.addAttack;
        playerMove.moveSpeed = 5f + GlobalData.addSpeed;

        if (currentLives < maxLives)
        {
            currentLives = maxLives;
        }

        Debug.Log($"Aktualne statystyki: MaxLives={maxLives}, Attack={attackDamage}, Speed={playerMove.moveSpeed}");
    }

    IEnumerator PerformAttack()
    {
        sword.SetActive(true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(0.3f);

        sword.SetActive(false);
    }

    void UpdateSwordPosition()
    {
        if (sword == null) return;

        Vector3 swordPosition = playerMove.transform.position;

        if (playerMove != null)
        {
            swordPosition = playerMove.sword.transform.position;
        }

        sword.transform.position = swordPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null)
            {
                
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.damage);
                Debug.Log("Took damage!");
            }
        }
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
