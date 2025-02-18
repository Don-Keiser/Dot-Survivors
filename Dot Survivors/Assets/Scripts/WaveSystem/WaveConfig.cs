using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Config", menuName = "ScriptableObjects/Wave Config", order = 1)]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnInfo 
    {
        public GameObject enemyPrefab;
        public float spawnChance;
    }

    public EnemySpawnInfo[] enemies;
    public float spawnRate;
    public float waveDuration;
}
