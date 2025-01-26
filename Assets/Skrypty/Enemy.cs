using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int maxHealth = 5; 
    private int currentHealth;
    public int damage = 1; 

    public GameObject coinPrefab;  
    public Transform coinSpawnPoint;  

    public void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.RegisterEnemy(this);
    }

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
    }
}
