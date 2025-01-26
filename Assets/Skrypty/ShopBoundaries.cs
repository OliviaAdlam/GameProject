using UnityEngine;

public class ShopBoundaries : MonoBehaviour
{
    [SerializeField] private SpriteRenderer groundSprite;
    [SerializeField] private float wallThickness = 1f;

    private void Start()
    {
        if (groundSprite == null)
        {
            groundSprite = GetComponent<SpriteRenderer>();
        }

        if (groundSprite != null)
        {
            CreateBoundaries();
        }
        else
        {
            Debug.LogError("No SpriteRenderer found for ground!");
        }
    }

    void CreateBoundaries()
    {
        // Get the actual size of the ground sprite in world units
        float mapWidth = groundSprite.bounds.size.x;
        float mapHeight = groundSprite.bounds.size.y;
        Vector3 center = groundSprite.bounds.center;

        // Create walls at the edges of the sprite
        CreateWall(new Vector2(center.x, center.y + mapHeight/2), new Vector2(mapWidth, wallThickness), "TopWall");
        CreateWall(new Vector2(center.x, center.y - mapHeight/2), new Vector2(mapWidth, wallThickness), "BottomWall");
        CreateWall(new Vector2(center.x - mapWidth/2, center.y), new Vector2(wallThickness, mapHeight), "LeftWall");
        CreateWall(new Vector2(center.x + mapWidth/2, center.y), new Vector2(wallThickness, mapHeight), "RightWall");
    }

    void CreateWall(Vector2 position, Vector2 size, string name)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = position;
        wall.tag = "Wall";

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = true;

        wall.AddComponent<WallCollision>();

        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
} 