using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public GameObject layerPrefab;
        public float parallaxSpeed = 0.1f;

        [Header("Color Shift Settings")]
        public bool enableColorShift = false;
        public Color startColor = Color.blue;
        public Color endColor = Color.magenta;
        public float colorShiftSpeed = 0.2f;
        
        [HideInInspector] public Transform[,] tiles;
        [HideInInspector] public float tileSizeX, tileSizeY;
        private SpriteRenderer layerRenderer;
        private float shiftOffset;

        public void Initialize()
        {
            SpriteRenderer sr = layerPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                layerRenderer = sr;
                layerRenderer.color = startColor;
            }
            
            shiftOffset = Random.Range(0f, 1f);
        }

        public void UpdateEffects()
        {
            if (enableColorShift)
            {
                float t = Mathf.PingPong(Time.time * colorShiftSpeed + shiftOffset, 1f);
                Color newColor = Color.Lerp(startColor, endColor, t);

                foreach (Transform tile in tiles)
                {
                    if (tile != null)
                    {
                        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            sr.color = newColor;
                        }
                    }
                }
            }
        }
    }

    public ParallaxLayer[] layers;
    [SerializeField] private Transform player;
    private Vector2 lastPlayerPosition;
    private const int GridSize = 4;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            enabled = false;
            return;
        }

        lastPlayerPosition = player.position;

        foreach (var layer in layers)
        {
            SpriteRenderer sr = layer.layerPrefab.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("Layer prefab is missing a SpriteRenderer!");
                continue;
            }

            layer.tileSizeX = sr.bounds.size.x;
            layer.tileSizeY = sr.bounds.size.y;
            layer.tiles = new Transform[GridSize, GridSize];

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    Vector2 spawnPos = new Vector2(
                        (x - 1) * layer.tileSizeX,
                        (y - 1) * layer.tileSizeY
                    );
                    GameObject tile = Instantiate(layer.layerPrefab, spawnPos, Quaternion.identity, transform);
                    layer.tiles[x, y] = tile.transform;
                }
            }
        }
    }

    private void Update()
{
    Vector2 playerDelta = (Vector2)player.position - lastPlayerPosition;

    foreach (var layer in layers)
    {
        if (layer.tiles == null) continue;

        foreach (Transform tile in layer.tiles)
        {
            tile.position += (Vector3)(playerDelta * layer.parallaxSpeed);
        }

        layer.UpdateEffects();
        CheckAndShiftLayer(layer);
    }

    lastPlayerPosition = player.position;
}

    private void CheckAndShiftLayer(ParallaxLayer layer)
    {
        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Transform tile = layer.tiles[x, y];
                Vector2 distance = (Vector2)player.position - (Vector2)tile.position;

                if (Mathf.Abs(distance.x) > layer.tileSizeX * (GridSize / 2))
                {
                    int direction = distance.x > 0 ? 1 : -1;
                    ShiftColumn(layer, direction);
                }

                if (Mathf.Abs(distance.y) > layer.tileSizeY * (GridSize / 2))
                {
                    int direction = distance.y > 0 ? 1 : -1;
                    ShiftRow(layer, direction);
                }
            }
        }
    }

    private void ShiftColumn(ParallaxLayer layer, int direction)
    {
        int oldIndex = direction > 0 ? 0 : GridSize - 1;
        int newIndex = direction > 0 ? GridSize - 1 : 0;

        for (int y = 0; y < GridSize; y++)
        {
            Transform oldTile = layer.tiles[oldIndex, y];
            Vector3 newPos = layer.tiles[newIndex, y].position + new Vector3(layer.tileSizeX * direction, 0, 0);
            oldTile.position = newPos;

            for (int x = oldIndex; x != newIndex; x += direction)
            {
                layer.tiles[x, y] = layer.tiles[x + direction, y];
            }

            layer.tiles[newIndex, y] = oldTile;
        }
    }

    private void ShiftRow(ParallaxLayer layer, int direction)
    {
        int oldIndex = direction > 0 ? 0 : GridSize - 1;
        int newIndex = direction > 0 ? GridSize - 1 : 0;

        for (int x = 0; x < GridSize; x++)
        {
            Transform oldTile = layer.tiles[x, oldIndex];
            Vector3 newPos = layer.tiles[x, newIndex].position + new Vector3(0, layer.tileSizeY * direction, 0);
            oldTile.position = newPos;

            for (int y = oldIndex; y != newIndex; y += direction)
            {
                layer.tiles[x, y] = layer.tiles[x, y + direction];
            }

            layer.tiles[x, newIndex] = oldTile;
        }
    }
}
