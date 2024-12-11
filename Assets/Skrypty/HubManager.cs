using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    public Text totalCoinsText; // Reference to the total coins display


    void Start()
    {
        UpdateTotalCoinsDisplay(); // Update the total coins display on start
    }

    public void StartAdventure()
    {
        SceneTransitionManager.Instance.LoadAdventureScene(); // Load the adventure scene
    }

    public void QuitGame()
    {
        SceneTransitionManager.Instance.QuitGame(); // Quit the game
    }

    // Update the total coins display UI with the value from GlobalData
    void UpdateTotalCoinsDisplay()
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text = $"Total Coins: {GlobalData.totalCoins}"; // Use the static class to get the total coins
        }
    }
}
