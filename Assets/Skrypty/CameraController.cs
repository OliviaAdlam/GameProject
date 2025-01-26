using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float leftBound, rightBound, bottomBound, topBound;
    private Camera cam;
    private Transform target;
    public float smoothSpeed = 5f;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 10f;
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void SetBounds(float left, float right, float bottom, float top)
    {
        leftBound = left;
        rightBound = right;
        bottomBound = bottom;
        topBound = top;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            float vertExtent = cam.orthographicSize;
            float horizExtent = vertExtent * Screen.width / Screen.height;

            // Calculate the bounds where the camera should stop
            float minX = leftBound + horizExtent;
            float maxX = rightBound - horizExtent;
            float minY = bottomBound + vertExtent;
            float maxY = topBound - vertExtent;

            // If the camera view is larger than the map, center it
            if (minX > maxX)
            {
                float x = (leftBound + rightBound) * 0.5f;
                minX = x;
                maxX = x;
            }
            if (minY > maxY)
            {
                float y = (bottomBound + topBound) * 0.5f;
                minY = y;
                maxY = y;
            }

            // Calculate desired position
            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(target.position.x, minX, maxX),
                Mathf.Clamp(target.position.y, minY, maxY),
                transform.position.z
            );

            // Move camera smoothly
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
} 