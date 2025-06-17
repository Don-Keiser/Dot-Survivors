using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 5)]
public class EnemyConfig : ScriptableObject
{
    public string enemyName;
    public float health;
    public float moveSpeed;
    public int damage;
    public float damageInterval;

    [Header("XP Orb Drop Settings")]
    public BonusConfig[] possibleDrops;
    public float[] dropChances;

    [Header("Splitter Settings")]
    public GameObject splitEnemyPrefab;
    public int splitCount = 0;

    public BonusConfig GetRandomDrop() 
    {
        if (possibleDrops.Length != dropChances.Length)
        {
            Debug.LogError("Possible drops and drop chances arrays must be of the same length.");
            return null;
        }

        float totalChance = 0f;
        foreach (float chance in dropChances)
        {
            totalChance += chance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        for (int i = 0; i < possibleDrops.Length; i++) 
        {
            cumulativeChance += dropChances[i];
            if (randomValue <= cumulativeChance) 
            {
                return possibleDrops[i];
            }
        }

        return null;
    }
}