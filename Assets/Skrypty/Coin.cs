using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1; // Wartość monety

    void Start()
    {
        // Register the coin when created
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterCoin();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collected the coin
        if (other.CompareTag("Player"))
        {
            // Add coin value to player score
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddCoins(value);
            }

            // Notify GameManager that coin was collected
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CoinCollected();
            }

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
