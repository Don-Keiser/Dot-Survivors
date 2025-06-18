using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeothsHaloAura : MonoBehaviour
{
    public float damage;
    public float cooldown;
    public float healingMultiplier = 0.2f;

    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    private PlayerStats playerStats;

    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] Transform visualEffect;
    [SerializeField] float pulseSpeed = 1.5f;
    [SerializeField] float pulseAmount = 0.1f;
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    private bool hasEnemies = false;
    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;


    private void Start()
    {
        if (visualEffect != null)
        {
            spriteRenderer = visualEffect.GetComponent<SpriteRenderer>();
            baseScale = visualEffect.localScale;
            StartCoroutine(PulseEffect());
        }
        StartCoroutine(ApplyDamageAndHeal());
    }

    public void SetRange(float range)
    {
        if (circleCollider != null)
        {
            circleCollider.radius = range / 2f;
        }

        if (visualEffect != null)
        {
            float size = range;
            baseScale = new Vector3(size, size, 1);
            visualEffect.localScale = baseScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemiesInRange.Add(col.gameObject);
            hasEnemies = true;
            UpdateColor();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(col.gameObject);
            hasEnemies = enemiesInRange.Count > 0;
            UpdateColor();
        }
    }

    private IEnumerator ApplyDamageAndHeal()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float totalDamageDealt = 0f;

            foreach (GameObject enemy in new HashSet<GameObject>(enemiesInRange))
            {
                if (enemy != null)
                {
                    var e = enemy.GetComponent<Enemy>();
                    if (e != null)
                    {
                        e.TakeDamage(damage);
                        totalDamageDealt += damage;
                    }
                }
            }

            if (playerStats != null && totalDamageDealt > 0f)
                playerStats.Heal(totalDamageDealt * healingMultiplier);

            yield return new WaitForSeconds(cooldown - 0.5f);
        }
    }

    private IEnumerator PulseEffect()
    {
        while (true)
        {
            float time = 0f;
            while (time < 1f)
            {
                float scaleFactor = 1f + Mathf.Sin(time * Mathf.PI * 2f) * pulseAmount;
                visualEffect.localScale = baseScale * scaleFactor;
                time += Time.deltaTime * pulseSpeed;
                yield return null;
            }
        }
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = hasEnemies ? activeColor : inactiveColor;
    }
}
