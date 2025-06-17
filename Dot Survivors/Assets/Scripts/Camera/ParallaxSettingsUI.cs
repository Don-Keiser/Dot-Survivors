using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxSettingsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParallaxMenuBackground parallaxBackground;
    [SerializeField] private Transform startColorGrid;
    [SerializeField] private Transform endColorGrid;
    [SerializeField] private Transform prefabGrid;
    [SerializeField] private TMP_Text currentLayerText;

    [Header("Prefabs")]
    [SerializeField] private GameObject colorButtonPrefab;
    [SerializeField] private GameObject prefabButtonPrefab;

    [Header("Color Options")]
    [SerializeField] private List<Color> availableColors;

    private int currentLayerIndex = 0;

    private void Start()
    {
        PopulateColorGrid(startColorGrid, OnStartColorSelected);
        PopulateColorGrid(endColorGrid, OnEndColorSelected);
        PopulatePrefabGrid();
        UpdateLayerLabel();
    }

    public void SelectNextLayer()
    {
        currentLayerIndex = (currentLayerIndex + 1) % parallaxBackground.layers.Length;
        PopulatePrefabGrid();
        UpdateLayerLabel();
    }

    public void SelectPreviousLayer()
    {
        currentLayerIndex = (currentLayerIndex - 1 + parallaxBackground.layers.Length) % parallaxBackground.layers.Length;
        PopulatePrefabGrid();
        UpdateLayerLabel();
    }

    private void PopulateColorGrid(Transform grid, System.Action<Color> onClick)
    {
        foreach (Transform child in grid) Destroy(child.gameObject);

        foreach (var color in availableColors)
        {
            GameObject btnObj = Instantiate(colorButtonPrefab, grid);
            Image img = btnObj.GetComponent<Image>();
            img.color = new Color(color.r, color.g, color.b, 1f);
            btnObj.GetComponent<Button>().onClick.AddListener(() => onClick(color));
        }
    }

    private void PopulatePrefabGrid()
    {
        foreach (Transform child in prefabGrid) Destroy(child.gameObject);

        var layer = parallaxBackground.layers[currentLayerIndex];

        for (int i = 0; i < layer.availablePrefabs.Count; i++)
        {
            int index = i;
            GameObject btnObj = Instantiate(prefabButtonPrefab, prefabGrid);

            Transform iconChild = btnObj.transform.Find("Icon");
            if (iconChild != null && iconChild.TryGetComponent(out Image iconImage))
            {
                SpriteRenderer sr = layer.availablePrefabs[i].GetComponent<SpriteRenderer>();
                if (sr)
                {
                    iconImage.sprite = sr.sprite;
                    iconImage.rectTransform.localScale = Vector3.one * 0.9f;
                }
            }

            btnObj.GetComponent<Button>().onClick.AddListener(() => OnPrefabSelected(index));
        }
    }

    private void OnStartColorSelected(Color color)
    {
        var layer = parallaxBackground.layers[currentLayerIndex];
        layer.startColor = color;
    }

    private void OnEndColorSelected(Color color)
    {
        var layer = parallaxBackground.layers[currentLayerIndex];
        layer.endColor = color;
    }

    private void OnPrefabSelected(int index)
    {
        var layer = parallaxBackground.layers[currentLayerIndex];
        layer.SwitchPattern(index);
        parallaxBackground.RefreshLayer(currentLayerIndex);
    }

    private void UpdateLayerLabel()
    {
        currentLayerText.text = $"Layer {currentLayerIndex + 1}";
    }

    public void SaveParallaxSettings()
    {
        ParallaxConfigManager.SaveConfig(parallaxBackground.layers);
        Debug.Log("✅ Parallax config saved: " + PlayerPrefs.GetString("ParallaxConfig"));
    }

}