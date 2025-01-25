using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Prefab pickupa broni
    public float weaponOffset = 2f; // Distance in front of player

    void Start()
    {
        EquipWeaponToPlayer();
    }

    public void EquipWeaponToPlayer()
    {
        if (GlobalData.currentWeaponIndex >= 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return;

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null) return;

            if (playerStats.sword != null)
            {
                Destroy(playerStats.sword);
            }

            // Get player's facing direction or default to right
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            Vector2 direction = playerMove != null ? playerMove.GetLastMovementDirection() : Vector2.right;
            if (direction == Vector2.zero) direction = Vector2.right; // Default direction if no movement

            // Calculate spawn position in front of the player
            Vector3 spawnPosition = player.transform.position + (Vector3)(direction.normalized * weaponOffset);

            // Podstawowa rotacja broni
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            GameObject newWeapon = Instantiate(weaponPrefabs[GlobalData.currentWeaponIndex], 
                spawnPosition, 
                Quaternion.Euler(0, 0, angle));

            // Set up references
            Weapon weaponComponent = newWeapon.GetComponent<Weapon>();
            if (weaponComponent != null)
            {
                weaponComponent.SetInitialPosition(spawnPosition);
                weaponComponent.SetPlayer(player);
                
                // Tworzymy attack point
                GameObject attackPointObj = new GameObject("AttackPoint");
                attackPointObj.transform.parent = newWeapon.transform;
                
                // Pozycja attack pointa zostanie ustawiona w Weapon.Start()
                playerStats.attackPoint = attackPointObj.transform;
            }

            playerStats.sword = newWeapon;

            if (playerMove != null)
            {
                playerMove.sword = newWeapon;
            }
        }
    }
}
