using UnityEngine;

public interface IParallaxLayer
{
    Color startColor { get; set; }
    Color endColor { get; set; }
    int currentPrefabIndex { get; set; }

    void Initialize();
    void UpdateEffects(float time = 0f);
}