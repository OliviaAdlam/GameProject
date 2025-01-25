using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 5; // Maximum health of the enemy
    private int currentHealth;
    public int damage = 1;   // Damage dealt by enemy

    public GameObject coinPrefab;  // Reference to coin prefab
    public Transform coinSpawnPoint;  // Where the coin should spawn

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Start with full health
        GameManager.Instance.RegisterEnemy(this); // Register this enemy with the GameManager
    }

    public void TakeDamage(int damage)
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

        // Handle coin spawning if needed
        if (coinPrefab != null && coinSpawnPoint != null)
        {
            // Instantiate a coin at the spawn point
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
            
        }

        // Destroy the enemy game object
        Destroy(gameObject); // Remove the enemy from the game

        // Notify the GameManager that the enemy has been defeated
        GameManager.Instance.EnemyDefeated(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Add any additional enemy logic here
    }
