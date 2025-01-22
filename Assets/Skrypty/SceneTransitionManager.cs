using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public void LoadHubScene()
    {
        SceneManager.LoadScene("Dom");
        
    }

    public void LoadAdventureScene()
    {
        Debug.Log("Do boju");
        SceneManager.LoadScene("Pole walki");
        Debug.Log("boju");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
