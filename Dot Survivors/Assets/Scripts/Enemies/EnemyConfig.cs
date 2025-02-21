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
    public XPOrbConfig[] possibleDrops;
    public float[] dropChances;

    public XPOrbConfig GetRandomDrop() 
    {
        float totalChance = 0f;

        foreach (float chance in dropChances)
        {
            float randomValue = Random.Range(0f, totalChance);
            float cumulativeChance = 0;

            for (int i = 0; i < possibleDrops.Length; i++) 
            {
                cumulativeChance += dropChances[i];
                if (randomValue <= cumulativeChance) 
                {
                    return possibleDrops[i];
                }
            }
        }
        return null;
    }
}
