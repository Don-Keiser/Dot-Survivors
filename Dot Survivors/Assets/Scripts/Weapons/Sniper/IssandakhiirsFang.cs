using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IssandakhiirsFang", menuName = "ScriptableObjects/IssandakhiirsFang")]
public class IssandakhiirsFang : WeaponBase
{
    [SerializeField] GameObject explosionProjectilePrefab;
    [SerializeField] float range;
    [SerializeField] int consecutiveShots = 1;
    [SerializeField] float explosionRadius = 1.5f;

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
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    GameObject proj = Instantiate(explosionProjectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
                    proj.GetComponent<Rigidbody2D>().linearVelocity = direction * (10f * PlayerPassives.Instance.GetProjectileSpeedMultiplier());

                    ExplosionOnHitProjectile script = proj.GetComponent<ExplosionOnHitProjectile>();
                    script.damage = GetModifiedDamage();
                    script.explosionRadius = explosionRadius;
                }
            }

            cooldownTimer = cooldown;
        }
    }

    private List<GameObject> FindClosestEnemies(Transform player, int count)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<KeyValuePair<GameObject, float>> enemyDistances = new();

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, player.position);
            if (dist <= range)
                enemyDistances.Add(new(enemy, dist));
        }

        enemyDistances.Sort((a, b) => a.Value.CompareTo(b.Value));

        List<GameObject> closest = new();
        for (int i = 0; i < Mathf.Min(count, enemyDistances.Count); i++)
            closest.Add(enemyDistances[i].Key);

        return closest;
    }

    protected override string[] GetPossibleUpgradeStats() =>
        new string[] { "damageIncrease", "cooldownReduction", "rangeIncrease", "extraShots", "explosionRadiusIncrease" };

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        range += upgrade.GetUpgradeValue("rangeIncrease");
        consecutiveShots += (int)upgrade.GetUpgradeValue("extraShots");
        explosionRadius += upgrade.GetUpgradeValue("explosionRadiusIncrease");
    }

    public override WeaponBase Clone()
    {
        IssandakhiirsFang copy = Instantiate(this);
        copy.level = level;
        copy.baseDamage = baseDamage;
        copy.cooldown = cooldown;
        copy.range = range;
        copy.consecutiveShots = consecutiveShots;
        copy.explosionRadius = explosionRadius;
        return copy;
    }
}