using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public bool isPiercing;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (!isPiercing)
            {
                Destroy(gameObject);
            }
        }
        else if (!isPiercing)
        {
            Destroy(gameObject);
        }
    }
}