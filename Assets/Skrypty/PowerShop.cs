using UnityEngine;

public class PowerShop : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject[] weaponPrefabs; // Array of available weapon prefabs

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

    public void BuyWeapon(int weaponIndex, int price)
    {
        if (GlobalData.totalCoins >= price)
        {
            if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
            {
                Debug.LogError("Invalid weapon index!");
                return;
            }

            GlobalData.totalCoins -= price;
            GlobalData.currentWeaponIndex = weaponIndex; // Set the current weapon index
            EquipNewWeapon(weaponPrefabs[weaponIndex]);
            Debug.Log($"Bought and equipped weapon index: {weaponIndex}");
        }
        else
        {
            Debug.Log("Za mało monet!");
        }
    }

    private void EquipNewWeapon(GameObject weaponPrefab)
    {
        // Destroy current weapon if it exists
        if (playerStats.sword != null)
        {
            Destroy(playerStats.sword);
        }

        // Get player's facing direction or default to right
        PlayerMove playerMove = playerStats.GetComponent<PlayerMove>();
        Vector2 direction = playerMove != null ? playerMove.GetLastMovementDirection() : Vector2.right;
        if (direction == Vector2.zero) direction = Vector2.right;

        // Calculate spawn position with offset
        float weaponOffset = 2f;
        Vector3 spawnPosition = playerStats.transform.position + (Vector3)(direction.normalized * weaponOffset);

        // Calculate rotation
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

        // Create and equip new weapon with correct position and rotation
        GameObject newWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
        playerStats.sword = newWeapon;

        // Set up weapon component and update global weapon data
        Weapon weaponComponent = newWeapon.GetComponent<Weapon>();
        if (weaponComponent != null)
        {
            weaponComponent.SetInitialPosition(spawnPosition);
            weaponComponent.SetPlayer(playerStats.gameObject);
            
            // Update global weapon data
            GlobalData.currentWeaponName = weaponComponent.weaponName;
            GlobalData.currentWeaponDamage = weaponComponent.damage;
            GlobalData.currentWeaponSpeed = weaponComponent.attackSpeed;
            
            // Update player's attack damage to include weapon damage
            playerStats.attackDamage = GlobalData.addAttack + (int)weaponComponent.damage;
        }

        // Update attack point
        Transform newAttackPoint = newWeapon.transform;
        if (newAttackPoint != null)
        {
            playerStats.attackPoint = newAttackPoint;
        }
        else
        {
            Debug.LogError("AttackPoint nie znaleziony w prefabie nowej broni!");
        }

        // Update the sword reference in PlayerMove
        if (playerMove != null)
        {
            playerMove.sword = newWeapon;
        }
    }

    // Add these specific methods for each weapon
    public void BuyWeapon1()
    {
        BuyWeapon(0, 10); // First weapon, costs 10
    }

    public void BuyWeapon2()
    {
        BuyWeapon(1, 200); // Second weapon, costs 20
    }

    public void BuyWeapon3()
    {
        BuyWeapon(2, 300); // Third weapon, costs 30
    }
}
