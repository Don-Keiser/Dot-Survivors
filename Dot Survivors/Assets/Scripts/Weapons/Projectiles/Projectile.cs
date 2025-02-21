using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public float damage;
    public bool isPiercing;
    public float lifetime = 5f;

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>(); // Prevents multiple hits

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hitEnemies.Contains(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                hitEnemies.Add(collision.gameObject);
            }

            if (!isPiercing)
            {
                Destroy(gameObject);
            }
        }
    }
}
