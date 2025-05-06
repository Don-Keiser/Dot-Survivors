using UnityEngine;
using System.Collections.Generic;

public class ParallaxMenuBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public List<GameObject> availablePrefabs;
        public int currentPrefabIndex = 0;
        [HideInInspector] public GameObject layerPrefab;

        public float parallaxSpeed = 0.1f;

        [Header("Color Shift Settings")]
        public bool enableColorShift = false;
        public Color startColor = Color.blue;
        public Color endColor = Color.magenta;
        public float colorShiftSpeed = 0.2f;

        [HideInInspector] public GameObject[,] tiles;
        [HideInInspector] public float tileWidth, tileHeight;
        private float shiftOffset;

        public void Initialize()
        {
            if (availablePrefabs.Count > 0)
                layerPrefab = availablePrefabs[currentPrefabIndex];
            else
                Debug.LogError("No prefabs assigned for ParallaxLayer.");

            shiftOffset = Random.Range(0f, 1f);
        }

        public void UpdateEffects(float time)
        {
            if (!enableColorShift) return;

            float t = Mathf.PingPong(time * colorShiftSpeed + shiftOffset, 1f);
            Color color = Color.Lerp(startColor, endColor, t);

            foreach (GameObject tile in tiles)
            {
                if (tile.TryGetComponent(out SpriteRenderer sr))
                {
                    sr.color = color;
                }
            }
        }

        public void SwitchPattern(int newIndex)
        {
            if (newIndex < 0 || newIndex >= availablePrefabs.Count) return;
            currentPrefabIndex = newIndex;
            layerPrefab = availablePrefabs[currentPrefabIndex];
        }
    }

    public ParallaxLayer[] layers;
    public int gridSize = 3;
    public Vector2 simulatedScrollSpeed = new Vector2(20f, 10f);

    private Vector2 scrollOffset;

    private void Start()
    {
        foreach (var layer in layers)
        {
            layer.Initialize();

            SpriteRenderer sr = layer.layerPrefab.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("Missing SpriteRenderer component on layer prefab");
                continue;
            }

            layer.tileWidth = sr.bounds.size.x;
            layer.tileHeight = sr.bounds.size.y;

            layer.tiles = new GameObject[gridSize, gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Vector2 pos = new Vector2(
                        (x - gridSize / 2f) * layer.tileWidth,
                        (y - gridSize / 2f) * layer.tileHeight
                    );

                    GameObject tile = Instantiate(layer.layerPrefab, transform);
                    tile.SetActive(true);
                    tile.transform.localPosition = pos;
                    layer.tiles[x, y] = tile;
                }
            }

            // layer.layerPrefab.SetActive(false);
        }
    }

    private void Update()
    {
        scrollOffset += simulatedScrollSpeed * Time.deltaTime;

        foreach (var layer in layers)
        {
            float px = scrollOffset.x * layer.parallaxSpeed;
            float py = scrollOffset.y * layer.parallaxSpeed;

            float w = layer.tileWidth;
            float h = layer.tileHeight;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    GameObject tile = layer.tiles[x, y];

                    float tx = ((x - gridSize / 2f) * w + px) % (w * gridSize);
                    float ty = ((y - gridSize / 2f) * h + py) % (h * gridSize);

                    if (tx > w * (gridSize / 2f)) tx -= w * gridSize;
                    if (ty > h * (gridSize / 2f)) ty -= h * gridSize;

                    tile.transform.localPosition = new Vector2(tx, ty);
                }
            }

            layer.UpdateEffects(Time.time);
        }
    }

    public void RefreshLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex >= layers.Length) return;

        var layer = layers[layerIndex];

        foreach (var tile in layer.tiles)
        {
            if (tile != null)
                Destroy(tile);
        }

        SpriteRenderer sr = layer.layerPrefab.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Missing SpriteRenderer component on layer prefab");
            return;
        }

        layer.tileWidth = sr.bounds.size.x;
        layer.tileHeight = sr.bounds.size.y;

        layer.tiles = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 pos = new Vector2(
                    (x - gridSize / 2f) * layer.tileWidth,
                    (y - gridSize / 2f) * layer.tileHeight
                );

                GameObject tile = Instantiate(layer.layerPrefab, transform);
                tile.SetActive(true);
                tile.transform.localPosition = pos;
                layer.tiles[x, y] = tile;
            }
        }
    }
}