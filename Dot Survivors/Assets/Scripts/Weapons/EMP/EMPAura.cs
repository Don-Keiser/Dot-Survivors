using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAura : MonoBehaviour
{
    public float damage;
    public float cooldown;
    private HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

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

        StartCoroutine(ApplyDamage());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
            hasEnemies = true;
            UpdateColor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
            hasEnemies = enemiesInRange.Count > 0;
            UpdateColor();
        }
    }

    private IEnumerator ApplyDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            foreach (GameObject enemy in new HashSet<GameObject>(enemiesInRange))
            {
                if (enemy != null)
                {
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.TakeDamage(damage);
                    }
                }
            }

            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(cooldown - 1.5f);
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
        {
            spriteRenderer.color = hasEnemies ? activeColor : inactiveColor;
        }
    }
}
