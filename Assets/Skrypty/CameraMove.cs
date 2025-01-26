using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector2 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector2(0, 0);
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y + offset.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
