using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "ScriptableObjects/ProjectileWeapon", order = 1)]
public class ProjectileWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public bool isPiercing;
    public float range;
    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            GameObject target = FindClosestEnemy(player);
            if (target != null)
            {
                Vector2 direction = (target.transform.position - firePoint.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
                projectile.GetComponent<Projectile>().damage = damage;
                projectile.GetComponent<Projectile>().isPiercing = isPiercing;
                cooldownTimer = cooldown;
                Debug.Log($"Fired at {target.name}. Cooldown reset to {cooldown}");
            }
            else
            {
                Debug.Log("No target found within range.");
            }
        }
    }

    public override void UpgradeWeapon()
    {
        level++;
        damage += 5f;
        cooldown *= 0.9f;
        range += 1f;
    }

    private GameObject FindClosestEnemy(Transform player)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPos = player.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, playerPos);
            if (distance < minDistance && distance <= range)
            {
                closest = enemy;
                minDistance = distance;
            }
        }

        if (closest != null)
        {
            Debug.Log($"Closest enemy found at distance: {minDistance}");
        }

        return closest;
    }
}