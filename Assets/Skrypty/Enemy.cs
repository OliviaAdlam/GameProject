using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int maxHealth = 5; // Maximum health of the enemy
    private int currentHealth;
    public int damage = 1;   // Damage dealt by enemy

    public GameObject coinPrefab;  // Reference to coin prefab
    public Transform coinSpawnPoint;  // Where the coin should spawn

    void Start()
    {
        currentHealth = maxHealth; // Start with full health
        GameManager.Instance.RegisterEnemy(this); // Register this enemy with the GameManager
    }

    // Oznacz metodę jako virtual, aby można ją było nadpisywać
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage! Health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        if (coinPrefab != null && coinSpawnPoint != null)
        {
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        }

        Destroy(gameObject);
        GameManager.Instance.EnemyDefeated();
    }

    void Update()
    {
        // Add any additional enemy logic here
    }
}
