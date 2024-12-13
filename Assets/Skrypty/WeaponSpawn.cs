using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public GameObject weaponPickupPrefab; // Prefab pickupa broni
    public Vector2[] spawnLocations; // Lista miejsc, w których pojawią się bronie

    void Start()
    {
        SpawnWeapons();
    }

    void SpawnWeapons()
    {
        foreach (Vector2 spawnLocation in spawnLocations)
        {
            Instantiate(weaponPickupPrefab, spawnLocation, Quaternion.identity);
        }
    }
}
