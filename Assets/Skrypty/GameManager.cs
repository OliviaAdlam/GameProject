using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int enemyCount = 0;
    private int coinCount = 0;
    private int currentWave = 1;
    private int currentAdventureCoins = 0;

    public GameObject nextWavePanel;
    public GameObject pausePanel;
    public Text totalCoinsText; // Reference to the total coins UI text

    public GameObject enemyPrefab;  
    public GameObject coinPrefab;   

    public int coinPercentageOnPauseReturn = 50; 

    private bool waveComplete = false;

    void Awake()
    {
        // Set up the singleton for the GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance of GameManager exists
        }
        DontDestroyOnLoad(gameObject); // Persist between scenes

        nextWavePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Start()
    {
        UpdateTotalCoinsUI(); // Ensure the total coins UI is updated
        if (pausePanel == null)
        {
            pausePanel = GameObject.Find("PausePanel"); // Find by name if not already assigned
        }

        if (nextWavePanel == null)
        {
            nextWavePanel = GameObject.Find("NextWavePanel");
        }

        if (pausePanel == null || nextWavePanel == null)
        {
            Debug.LogWarning("One or more UI elements could not be found!");
        }
    }

    void Update()
    {
        // Pause the game with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Methods to register enemies and coins (for gameplay logic)
    public void RegisterEnemy(Enemy enemy)
    {
        enemyCount++;
        Debug.Log($"Enemy registered. Current enemy count: {enemyCount}");
    }

    public void EnemyDefeated()
    {
        enemyCount--;
        Debug.Log($"Enemy defeated. Current enemy count: {enemyCount}");
        
        
    }

    public void RegisterCoin()
    {
        coinCount++;
        Debug.Log($"Coin registered. Current coin count: {coinCount}");
    }

    public void CoinCollected()
    {
        coinCount--;
        currentAdventureCoins++;
        Debug.Log($"Coin collected. Current adventure coins: {currentAdventureCoins}");
        UpdateTotalCoinsUI(); // Update the UI in real-time if applicable
        CheckWaveCompletion();
    }

    // Check if the wave is complete and handle it
    void CheckWaveCompletion()
    {
        Debug.Log($"Checking wave completion. Enemy count: {enemyCount}, Coin count: {coinCount}, Wave complete: {waveComplete}");
        if (enemyCount <= 0 && coinCount <= 0 && !waveComplete)
        {
            waveComplete = true;
            ShowNextWavePanel();
        }
    }

    // Show the next wave panel and pause the game
    void ShowNextWavePanel()
    {
        if (nextWavePanel != null)
        {
            Time.timeScale = 0; // Pause the game
            nextWavePanel.SetActive(true);
            Debug.Log("Wave Complete! Showing next wave panel.");
        }
    }

    // Start the next wave and reset relevant data
    public void StartNextWave()
    {
        waveComplete = false;
        nextWavePanel.SetActive(false);
        Time.timeScale = 1;

        currentWave++;
        enemyCount = 0;
        coinCount = 0;

        SpawnNewWave();
    }

    // Handle the process of returning to the hub, adding coins to the global total
    public void ReturnToHub(bool fullReward)
    {
        int coinsToAdd = fullReward ? currentAdventureCoins : Mathf.FloorToInt(currentAdventureCoins * (coinPercentageOnPauseReturn / 100f));
        GlobalData.totalCoins += coinsToAdd;  // Update the static global total coins

        Debug.Log($"Returning to hub. Coins added: {coinsToAdd}. Total coins: {GlobalData.totalCoins}");

        ResetAdventure();
        UpdateTotalCoinsUI();

        SceneTransitionManager.Instance.LoadHubScene(); // Go back to the hub
    }

    // Reset adventure-specific data
    void ResetAdventure()
    {
        waveComplete = false;
        nextWavePanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        enemyCount = 0;
        coinCount = 0;

        currentAdventureCoins = 0; // Reset coins gathered in adventure
        currentWave = 1;
    }

    // Handle player death, and return to hub
    public void PlayerDied()
    {
        Debug.Log("Player died. Returning to hub with no coins.");
        ResetAdventure();
        SceneTransitionManager.Instance.LoadHubScene(); // Go back to the hub
    }

    // Spawn new wave of enemies and coins
    void SpawnNewWave()
    {
        int numEnemies = 5 + currentWave * 2;  
        int numCoins = 5 + currentWave * 2;

        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        for (int i = 0; i < numCoins; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Toggle pause menu visibility
    public void TogglePause()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update the UI with the total coins from the static class
    void UpdateTotalCoinsUI()
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text = $"Total Coins: {GlobalData.totalCoins}";  // Use the static global total coins
        }
    }
}
