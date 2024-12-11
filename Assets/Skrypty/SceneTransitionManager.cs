using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public void LoadHubScene()
    {
        SceneManager.LoadScene("HubScene");
    }

    public void LoadAdventureScene()
    {
        SceneManager.LoadScene("Pole walki");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
