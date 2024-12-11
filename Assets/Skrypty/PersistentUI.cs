using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static PersistentUI _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Make this Canvas persist
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
