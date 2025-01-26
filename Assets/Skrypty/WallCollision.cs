using UnityEngine;

public class WallCollision : MonoBehaviour
{
    private bool isBoundaryWall;
    private Vector3 lastValidPosition;

    void Start()
    {
        // Check if this is a boundary wall by name
        isBoundaryWall = gameObject.name.Contains("Wall");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isBoundaryWall) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Store the last valid position before collision
            lastValidPosition = other.transform.position;

            // For enemies, handle collision based on their type
            if (other.CompareTag("Enemy"))
            {
                Smol_Enemy smallEnemy = other.GetComponent<Smol_Enemy>();
                Big_Enemy bigEnemy = other.GetComponent<Big_Enemy>();

                if (smallEnemy != null)
                {
                    smallEnemy.ChangeRandomDirection();
                }
                else if (bigEnemy != null)
                {
                    // Force big enemy to move in opposite direction
                    Vector3 awayFromWall = (other.transform.position - transform.position).normalized;
                    other.transform.position = lastValidPosition + awayFromWall * 1f;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isBoundaryWall) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Always keep the last valid position updated
            Vector3 currentPos = other.transform.position;
            
            // Only restrict movement on the axis we're colliding with
            if (gameObject.name.Contains("Left") || gameObject.name.Contains("Right"))
            {
                currentPos.x = lastValidPosition.x;
            }
            else if (gameObject.name.Contains("Top") || gameObject.name.Contains("Bottom"))
            {
                currentPos.y = lastValidPosition.y;
            }
            
            other.transform.position = currentPos;

            // Additional check for big enemy to prevent getting stuck
            Big_Enemy bigEnemy = other.GetComponent<Big_Enemy>();
            if (bigEnemy != null)
            {
                // Force position to last valid position if too far from wall
                float distanceFromWall = Vector3.Distance(transform.position, other.transform.position);
                if (distanceFromWall > 2f)
                {
                    other.transform.position = lastValidPosition;
                }
            }
        }
    }
} 