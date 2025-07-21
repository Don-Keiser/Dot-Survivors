using UnityEngine;
using System.Collections.Generic;

public class ExplosionOnHitProjectile : MonoBehaviour
{
    public float damage;
    public float explosionRadius = 1.5f;
    public bool isPiercing;
    public GameObject explosionEffectPrefab;

    private HashSet<GameObject> hitEnemies = new();

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || hitEnemies.Contains(collision.gameObject))
            return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hitEnemies.Add(collision.gameObject);

            // Spawn explosion
            if (explosionEffectPrefab != null)
            {
                GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                explosion.GetComponent<ExplosionEffect>().Initialize(explosionRadius);
            }

            // AoE damage
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") && hit.gameObject != collision.gameObject)
                {
                    Enemy aoeEnemy = hit.GetComponent<Enemy>();
                    aoeEnemy?.TakeDamage(damage * 0.5f); // splash is 50%
                }
            }

            if (!isPiercing)
                Destroy(gameObject);
        }
    }
}