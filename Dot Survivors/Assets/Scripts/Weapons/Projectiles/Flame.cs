using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flame : MonoBehaviour
{
    private float flameRange;
    private float coneAngle;
    private float damagePerSecond;
    private float lifetime = 2f; // How long the flame lasts
    private int direction; // -1 for left, 1 for right

    public void Initialize(float range, float angle, float damage, int dir)
    {
        flameRange = range;
        coneAngle = angle;
        damagePerSecond = damage;
        direction = dir;
        Destroy(gameObject, lifetime); // Auto-destroy flame after time
    }

    private void Update()
    {
        DealDamageToEnemies();
    }

    private void DealDamageToEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, flameRange);

        foreach (Collider2D collider in hitEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = (collider.transform.position - transform.position).normalized;
                float angleToEnemy = Vector2.Angle(transform.right * direction, directionToEnemy);

                if (angleToEnemy <= coneAngle / 2) // Check if enemy is in the cone
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damagePerSecond * Time.deltaTime);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flameRange);
    }
}
