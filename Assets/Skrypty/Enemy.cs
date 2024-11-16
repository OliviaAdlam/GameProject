
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 5; //Maksymalne życie gamonia
    private int currentHealth;
    public int damage = 1;   

    public GameObject coinPrefab;  // referencja coina
    public Transform coinSpawnPoint;  // gdzie ma być coin


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //Na początku jest bez ran
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        Debug.Log("Wróg dostał w pape ale gupek! Pozostało mu: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Wróg pad i nie wstał");
        Destroy(gameObject); //usuwa gamonia z gry

        if (coinPrefab != null && coinSpawnPoint != null)
        {
            // instancja coina w spawn pointcie
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
