using System.Collections.Generic;
using UnityEngine;


public static class ParallaxConfigManager
{
    private const string SaveKey = "ParallaxConfig";

    public static void SaveConfig<T>(T[] layers) where T : IParallaxLayer
    {
        var config = new ParallaxConfig();

        foreach (var layer in layers)
        {
            config.layers.Add(new ParallaxLayerConfig
            {
                startColor = layer.startColor,
                endColor = layer.endColor,
                prefabIndex = layer.currentPrefabIndex
            });
        }

        string json = JsonUtility.ToJson(config);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.Log("✅ Parallax config saved: " + json);
    }

    public static void LoadConfig<T>(T[] layers, System.Action<int> refreshCallback = null) where T : IParallaxLayer
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            Debug.LogWarning("[LoadConfig] No config found in PlayerPrefs");
            return;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        Debug.Log("[LoadConfig] JSON: " + json);

        var config = JsonUtility.FromJson<ParallaxConfig>(json);
        if (config == null || config.layers.Count != layers.Length)
        {
            Debug.LogError("[LoadConfig] Invalid config structure");
            return;
        }

        for (int i = 0; i < layers.Length; i++)
        {
            var layer = layers[i];
            var saved = config.layers[i];

            layer.startColor = saved.startColor;
            layer.endColor = saved.endColor;
            layer.currentPrefabIndex = saved.prefabIndex;

            Debug.Log($"Layer {i}: start={saved.startColor}, end={saved.endColor}, prefabIndex={saved.prefabIndex}");

            layer.Initialize();
            layer.UpdateEffects(Time.time);

            refreshCallback?.Invoke(i);
        }
    }
}