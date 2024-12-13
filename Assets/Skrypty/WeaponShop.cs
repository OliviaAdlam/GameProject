using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject newWeaponPrefab;
    public GameObject oldWeaponPrefab;
    public int weaponPrice; 
    public int gold=30;
    private bool isInRange = false; 
    private GameObject player; 

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            //PlayerStats stats = player.GetComponent<PlayerStats>();
            // wziac zloto globalne
            if ( gold >= weaponPrice) 
            {
                gold -= weaponPrice;
                EquipWeapon();
            }
            else
            {
                Debug.Log("Za mało!");
            }
        }
    }

    private void EquipWeapon()
    {
        Instantiate(newWeaponPrefab, player.transform.position, Quaternion.identity, player.transform);
        
        Destroy(gameObject);

        Instantiate(oldWeaponPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            player = null;
        }
    }
}
