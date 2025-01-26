using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    [System.Serializable]
    public class TileWithWeight
    {
        public Sprite sprite;
        public float weight = 1f;    // Higher weight = more common
        public bool hasCollision;     // Whether this tile should have collision
    }

    public TileWithWeight[] tiles;
    [SerializeField] public int width = 8;
    [SerializeField] public int height = 8;
    [SerializeField] public float tileSize = 1f;
    [SerializeField] public float scaleFactor = 4f;

    private void Awake()
    {
        // Force smaller map size regardless of Inspector values
        width = 16;
        height = 16;
        tileSize = 1f;
        scaleFactor = 4f;
    }

    private void OnEnable()
    {
        ClearBackground();
        GenerateBackground();
        CreateBoundaries();
    }

    private void OnDisable()
    {
        ClearBackground();
    }

    void ClearBackground()
    {
        // Remove all child objects
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    void CreateBoundaries()
    {
        float mapWidth = width * tileSize * scaleFactor;
        float mapHeight = height * tileSize * scaleFactor;
        float wallThickness = 1f;

        // Create walls exactly at the edges of the tiles
        CreateWall(new Vector2(0, mapHeight/2), new Vector2(mapWidth + wallThickness*2, wallThickness), "TopWall");
        CreateWall(new Vector2(0, -mapHeight/2), new Vector2(mapWidth + wallThickness*2, wallThickness), "BottomWall");
        CreateWall(new Vector2(-mapWidth/2, 0), new Vector2(wallThickness, mapHeight), "LeftWall");
        CreateWall(new Vector2(mapWidth/2, 0), new Vector2(wallThickness, mapHeight), "RightWall");

        // Set camera boundaries to match exactly with the map edges
        Camera.main.transform.position = new Vector3(0, 0, -10);
        if (Camera.main.GetComponent<CameraController>() == null)
        {
            CameraController controller = Camera.main.gameObject.AddComponent<CameraController>();
            controller.SetBounds(
                -mapWidth/2,
                mapWidth/2,
                -mapHeight/2,
                mapHeight/2
            );
        }
    }

    void CreateWall(Vector2 position, Vector2 size, string name)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = position;
        wall.tag = "Wall";  // Add this tag in Unity's Tag Manager

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = true;  // Keep as trigger for boundary walls

        // Add the WallCollision script
        wall.AddComponent<WallCollision>();

        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;  // Prevent any movement

        // Make walls visible but semi-transparent
        SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        sr.sortingOrder = 1;
    }

    void GenerateBackground()
    {
        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogError("No sprites assigned to BackgroundTiler!");
            return;
        }

        float totalWeight = 0;
        foreach (var tileWeight in tiles)
        {
            totalWeight += tileWeight.weight;
        }

        float startX = -width * tileSize * scaleFactor / 2;
        float startY = -height * tileSize * scaleFactor / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Pick a random tile based on weights
                float random = Random.Range(0f, totalWeight);
                float currentWeight = 0;
                TileWithWeight selectedTile = tiles[0];

                foreach (var tileWeight in tiles)
                {
                    currentWeight += tileWeight.weight;
                    if (random <= currentWeight)
                    {
                        selectedTile = tileWeight;
                        break;
                    }
                }

                // Create game object for the tile
                GameObject tileObject = new GameObject($"Tile_{x}_{y}");
                tileObject.transform.parent = transform;
                tileObject.layer = LayerMask.NameToLayer("Ground"); // Set to your ground/collision layer
                
                // Position with scale factor
                float posX = startX + (x * tileSize * scaleFactor);
                float posY = startY + (y * tileSize * scaleFactor);
                tileObject.transform.position = new Vector3(posX + (tileSize * scaleFactor / 2), posY + (tileSize * scaleFactor / 2), 0);
                
                // Set scale
                tileObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

                // Add sprite renderer
                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = selectedTile.sprite;
                spriteRenderer.sortingOrder = -1;  // Behind other objects

                // Add collider if needed
                if (selectedTile.hasCollision)
                {
                    BoxCollider2D collider = tileObject.AddComponent<BoxCollider2D>();
                    collider.size = Vector2.one; // Use unit size since we're scaling the object
                    collider.isTrigger = true;
                    
                    // Add a script to handle collision
                    WallCollision wallCollision = tileObject.AddComponent<WallCollision>();
                }
            }
        }
    }
}