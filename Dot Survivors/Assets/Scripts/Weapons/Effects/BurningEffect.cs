using UnityEngine;
using System.Collections;

public class BurningEffect : MonoBehaviour
{
    private float damagePerTick;
    private float duration;
    private float tickInterval = 0.4f;

    private static readonly Color[] flameColors = {
        new Color(1f, 0.2f, 0.1f),   // Red
        new Color(1f, 0.5f, 0f),     // Orange
        new Color(1f, 1f, 0f)        // Yellow
    };

    public void Initialize(float dmg, float dur)
    {
        damagePerTick = dmg;
        duration = dur;
        StartCoroutine(ApplyBurn());
    }

    private IEnumerator ApplyBurn()
    {
        float elapsed = 0f;
        var enemy = GetComponent<Enemy>();

        while (elapsed < duration && enemy != null)
        {
            enemy.TakeDamage(damagePerTick);
            SpawnFlameParticles(6);

            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        Destroy(this);
    }

    private void SpawnFlameParticles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.3f; // random small offset
            Vector3 spawnPosition = transform.position + (Vector3)offset;

            GameObject flame = Instantiate(Resources.Load<GameObject>("FlameParticle"), spawnPosition, Quaternion.identity);
            SpriteRenderer sr = flame.GetComponent<SpriteRenderer>();
            sr.color = flameColors[Random.Range(0, flameColors.Length)];
        }
    }
}
