using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector2(0, 0);
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, -10);
        transform.position = desiredPosition;
    }
}
