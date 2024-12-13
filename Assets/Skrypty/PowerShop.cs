using UnityEngine;

public class PowerShop : MonoBehaviour
{
    public PlayerStats playerStats;

    public void BuyExtraHeart(int price)
    {
        if (GlobalData.totalCoins >= price)
        {
            GlobalData.totalCoins -= price;
            GlobalData.addLives += 1;
            playerStats.ApplyUpgrades();
            Debug.Log("Kupiono dodatkowe życie!");
        }
        else
        {
            Debug.Log("Za mało monet!");
        }
    }

    public void BuyExtraAttack(int price)
    {
        if (GlobalData.totalCoins >= price)
        {
            GlobalData.totalCoins -= price;
            GlobalData.addAttack += 1;
            playerStats.ApplyUpgrades();
            Debug.Log("Kupiono dodatkowy atak!");
        }
        else
        {
            Debug.Log("Za mało monet!");
        }
    }

    public void BuyExtraSpeed(int price)
    {
        if (GlobalData.totalCoins >= price)
        {
            GlobalData.totalCoins -= price;
            GlobalData.addSpeed += 0.5f; // Dodajemy bonus do prędkości
            playerStats.ApplyUpgrades();
            Debug.Log("Kupiono dodatkową prędkość!");
        }
        else
        {
            Debug.Log("Za mało monet!");
        }
    }
}
