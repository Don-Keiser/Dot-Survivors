using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallaxLayerConfig
{
    public Color startColor;
    public Color endColor;
    public int prefabIndex;
}

[Serializable]
public class ParallaxConfig
{
    public List<ParallaxLayerConfig> layers = new();
}
