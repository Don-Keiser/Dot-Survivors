using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Vector2 targetPoint;
    private float explosionRadius;
    private float damage;
    private float speed;
    [SerializeField] private GameObject explosionEffectPrefab;

    public void Initialize(Vector2 target, float radius, float dmg, float moveSpeed)
    {
        targetPoint = target;
        explosionRadius = radius;
        damage = dmg;
        speed = moveSpeed;
    }

    private void Update()
    {
        // Move towards the target
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        // If reached the target, explode
        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Spawn expanding explosion circle
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            explosion.GetComponent<ExplosionEffect>().Initialize(explosionRadius);
        }

        // Check for enemies within the explosion radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        if (hitEnemies.Length == 0)
        {
            Debug.LogWarning("Grenade exploded, but no enemies were detected!");
        }

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log($"Grenade hit {enemy.name}, dealing {damage} damage.");
                    enemy.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("Enemy detected but missing Enemy script!");
                }
            }
            else
            {
                Debug.Log($"Explosion hit {collider.name}, but it's not an enemy.");
            }
        }

        Destroy(gameObject); // Remove grenade after explosion
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
