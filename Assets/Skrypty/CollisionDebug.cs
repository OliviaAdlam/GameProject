using UnityEngine;

public class CollisionDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Weszło: " + collision.name);
    }
}
