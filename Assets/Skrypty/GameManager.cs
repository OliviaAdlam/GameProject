using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject smolEnemyPrefab;  // Changed from enemyPrefab
    public GameObject bigEnemyPrefab;   // Added this
    public GameObject coinPrefab;

    public int coinPercentageOnPauseReturn = 50; 

    private bool waveComplete = false;

    // Define spawn boundaries
    private float minX = -25f;
    private float maxX = 25f;
    private float minY = -15f;
    private float maxY = 15f;
    private float minSpawnDistance = 8f; // Minimum distance between spawned enemies

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPos;
        bool validPosition = false;
        int maxAttempts = 30;
        int attempts = 0;

        do
        {
            // Get random position within boundaries
            spawnPos = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0
            );

            // Check distance from other enemies
            validPosition = true;
            Enemy[] existingEnemies = GameObject.FindObjectsOfType<Enemy>();
            
            foreach (Enemy enemy in existingEnemies)
            {
                if (Vector3.Distance(spawnPos, enemy.transform.position) < minSpawnDistance)
                {
                    validPosition = false;
                    break;
                }
            }

            attempts++;
        } while (!validPosition && attempts < maxAttempts);

        return spawnPos;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // If we have the old enemyPrefab reference, use it for smolEnemyPrefab
        if (smolEnemyPrefab == null && GetComponent<MonoBehaviour>().GetType().GetField("enemyPrefab") != null)
        {
            smolEnemyPrefab = (GameObject)GetComponent<MonoBehaviour>().GetType().GetField("enemyPrefab").GetValue(this);
            Debug.Log("Converted old enemyPrefab reference to smolEnemyPrefab");
        }

        // Load the big enemy prefab by path if it's not assigned
        if (bigEnemyPrefab == null)
        {
            bigEnemyPrefab = Resources.Load<GameObject>("MainAsset/Enemy 1");
            if (bigEnemyPrefab == null)
            {
                Debug.LogError("Failed to load big enemy prefab from Resources folder");
            }
        }

        // Create a store for our prefabs
        GameObject prefabStore = new GameObject("PrefabStore");
        prefabStore.transform.parent = transform;
        DontDestroyOnLoad(prefabStore);

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        if (scene.name == "Pole walki")
        {
            // Konfiguracja dla małych przeciwników
            Smol_Enemy[] smolEnemies = FindObjectsOfType<Smol_Enemy>();
            foreach (Smol_Enemy enemy in smolEnemies)
            {
                BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    collider.size = new Vector2(0.8f, 0.8f);  // Rozmiar dopasowany do sprite'a
                    collider.offset = Vector2.zero;
                    collider.isTrigger = false;  // Ważne - musi być false dla OnCollisionEnter2D
                }
            }

            // Istniejący kod dla dużych przeciwników...
            Big_Enemy[] bigEnemies = FindObjectsOfType<Big_Enemy>();
            foreach (Big_Enemy enemy in bigEnemies)
            {
                BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    collider.size = new Vector2(1f, 1f);  // Larger collider
                    collider.offset = Vector2.zero;
                    collider.isTrigger = false;  // Ensure it's not a trigger
                }
            }

            // Equip weapon as before
            WeaponSpawn weaponSpawn = FindObjectOfType<WeaponSpawn>();
            if (weaponSpawn != null)
            {
                weaponSpawn.EquipWeaponToPlayer();
                Debug.Log("Weapon equipped to player.");
            }
            else
            {
                Debug.LogError("WeaponSpawn not found in the scene.");
            }
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
        int numSmallEnemies = 3 + currentWave;
        int numBigEnemies = 1 + (currentWave / 2);
        int numCoins = 5 + currentWave;

        // Spawn small enemies
        if (smolEnemyPrefab != null)
        {
            for (int i = 0; i < numSmallEnemies; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject enemy = Instantiate(smolEnemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.localScale = new Vector3(5f, 5f, 1f);
                
                // Dodaj i skonfiguruj collider
                BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
                if (collider == null)
                {
                    collider = enemy.AddComponent<BoxCollider2D>();
                }
                collider.size = new Vector2(0.8f, 0.8f);
                collider.offset = Vector2.zero;
                collider.isTrigger = false;

                SpriteRenderer renderer = enemy.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 5;
                }
            }
        }
        else
        {
            Debug.LogError("Small enemy prefab is missing!");
        }

        // Spawn big enemies
        if (bigEnemyPrefab != null)
        {
            for (int i = 0; i < numBigEnemies; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject enemy = Instantiate(bigEnemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.localScale = new Vector3(4f, 4f, 4f);

                // Add BoxCollider2D adjustment
                BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    collider.size = new Vector2(1f, 1f);  // Larger collider
                    collider.offset = Vector2.zero;
                    collider.isTrigger = false;  // Ensure it's not a trigger
                }

                SpriteRenderer renderer = enemy.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 5;
                }
            }
        }
        else
        {
            Debug.LogError("Big enemy prefab is missing!");
        }

        // Spawn coins with null check
        if (coinPrefab != null)
        {
            for (int i = 0; i < numCoins; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                coin.transform.localScale = new Vector3(2f, 2f, 1f);
                RegisterCoin();
            }
        }
        else
        {
            Debug.LogError("Coin prefab is missing!");
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
