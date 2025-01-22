using UnityEngine;
using UnityEngine.UI;

public class HubSceneManager : MonoBehaviour
{
    public Button adventureButton; // Assign in Inspector
   

    void Start()
    {
        if (adventureButton != null)
        {
            adventureButton.onClick.RemoveAllListeners(); // Clear previous listeners
            adventureButton.onClick.AddListener(() => SceneTransitionManager.Instance.LoadAdventureScene());
        }
    }
}
