using UnityEngine;
using System.Collections;

public class HalooshsEchoGrenade : MonoBehaviour
{
    private Vector2 targetPoint;
    private float explosionRadius;
    private float damage;
    private float speed;

    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float echoDistanceMultiplier = 3f;
    [SerializeField] private float echoDelay = 0.2f;
    [SerializeField] private float echoDamageMultiplier = 0.5f;

    private Vector2 startPosition;

    public void Initialize(Vector2 target, float radius, float dmg, float moveSpeed)
    {
        targetPoint = target;
        explosionRadius = radius;
        damage = dmg;
        speed = moveSpeed;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            StartCoroutine(HandleExplosions());
        }
    }

    private IEnumerator HandleExplosions()
    {
        Vector2 direction = (targetPoint - startPosition).normalized;
        Vector2 echoPosition = targetPoint + direction * (explosionRadius * echoDistanceMultiplier);

        // Primary explosion
        ExplodeAt(transform.position, explosionRadius, damage);

        yield return new WaitForSeconds(echoDelay);

        // Echo explosion
        ExplodeAt(echoPosition, explosionRadius, damage * echoDamageMultiplier);

        Destroy(gameObject);
    }

    private void ExplodeAt(Vector2 position, float radius, float dmg)
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, position, Quaternion.identity)
                .GetComponent<ExplosionEffect>()
                .Initialize(radius);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.TakeDamage(dmg);
            }
        }
    }
}
