using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject newWeaponPrefab; // Prefab nowej broni
    public int weaponPrice; // Cena broni

    private bool isInRange = false; // Czy gracz jest w zasięgu
    private GameObject player; // Referencja do gracza

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E)) // Naciśnięcie klawisza E
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("Brak komponentu PlayerStats na graczu!");
                return;
            }

            // Sprawdzenie, czy gracz ma wystarczającą ilość złota
            if (GlobalData.totalCoins >= weaponPrice)
            {
                GlobalData.totalCoins -= weaponPrice; // Odejmowanie złota
                EquipWeapon(playerStats); // Wyposażenie gracza w broń
            }
            else
            {
                Debug.Log("Za mało złota!");
            }
        }
    }

    private void EquipWeapon(PlayerStats playerStats)
    {

    // Tworzenie nowej broni i przypisywanie jej do gracza
    GameObject newWeapon = Instantiate(newWeaponPrefab, playerStats.transform.position, Quaternion.identity, playerStats.transform);
    playerStats.sword = newWeapon; // Przypisanie nowej broni

    // Znalezienie `attackPoint` w nowej broni
    Transform newAttackPoint = newWeapon.transform;
    if (newAttackPoint != null)
    {
        playerStats.attackPoint = newAttackPoint; // Aktualizacja `attackPoint`
    }
    else
    {
        Debug.LogError("AttackPoint nie znaleziony w prefabie nowej broni!");
    }

    // Usuwanie pickupa broni
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Jeśli gracz wchodzi w zasięg
        {
            isInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Jeśli gracz wychodzi z zasięgu
        {
            isInRange = false;
            player = null;
        }
    }
}
