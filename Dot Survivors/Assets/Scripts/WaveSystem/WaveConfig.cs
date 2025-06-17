using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Config", menuName = "ScriptableObjects/Wave Config", order = 1)]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnInfo 
    {
        public GameObject enemyPrefab;
        [UnityEngine.Range(1, 100)] public float spawnChance;
    }

    public EnemySpawnInfo[] enemies;
    [UnityEngine.Range(0.01f, 2.0f)] public float spawnRate;
    [UnityEngine.Range(10, 300)] public float waveDuration;
}
