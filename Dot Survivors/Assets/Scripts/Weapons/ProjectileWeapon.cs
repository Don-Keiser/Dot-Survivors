using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "ScriptableObjects/ProjectileWeapon", order = 1)]
public class ProjectileWeapon : WeaponBase
{
    public GameObject projectilePrefab;
    public bool isPiercing;
    public float range;
    public int consecutiveShots = 1;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            List<GameObject> targets = FindClosestEnemies(player, consecutiveShots);
            foreach (var target in targets)
            {
                if (target != null)
                {
                    Vector2 direction = (target.transform.position - firePoint.position).normalized;
                    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
                    projectile.GetComponent<Projectile>().damage = damage;
                    projectile.GetComponent<Projectile>().isPiercing = isPiercing;
                }
            }
            cooldownTimer = cooldown;
            Debug.Log($"Fired {targets.Count} shots. Cooldown reset to {cooldown}");
        }
    }

    public override void UpgradeWeapon()
    {
        level++;
        damage += 5f;
        cooldown *= 0.9f;
        range += 1f;
        consecutiveShots++;
    }

    private List<GameObject> FindClosestEnemies(Transform player, int count)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> closestEnemies = new List<GameObject>();
        Vector3 playerPos = player.position;

        List<KeyValuePair<GameObject, float>> enemiesWithDistances = new List<KeyValuePair<GameObject, float>>();
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, playerPos);
            if (distance <= range)
            {
                enemiesWithDistances.Add(new KeyValuePair<GameObject, float>(enemy, distance));
            }
        }

        enemiesWithDistances.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        for (int i = 0; i < Mathf.Min(count, enemiesWithDistances.Count); i++)
        {
            closestEnemies.Add(enemiesWithDistances[i].Key);
        }

        return closestEnemies;
    }
}