using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunWeapon", menuName = "ScriptableObjects/ShotgunWeapon", order = 4)]
public class ShotgunWeapon : WeaponBase
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int pellets = 5;
    [SerializeField] float spreadAngle = 15f;
    [SerializeField] float bulletSpeed = 10f;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = cooldown;
            FireShotgun(firePoint);
        }
    }

    private void FireShotgun(Transform firePoint)
    {
        GameObject closestEnemy = FindClosestEnemy(firePoint.position);
        if (closestEnemy == null) return; // Don't shoot if no enemies are present

        // Get direction towards the closest enemy
        Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pellets; i++)
        {
            // Apply spread around the target enemy
            float pelletOffset = UnityEngine.Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, baseAngle + pelletOffset);

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, bulletRotation);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.right * (bulletSpeed * PlayerPassives.Instance.GetProjectileSpeedMultiplier());
            bullet.GetComponent<Projectile>().damage = GetModifiedDamage();
        }
    }

    // Finds the closest enemy to the fire point
    private GameObject FindClosestEnemy(Vector2 firePoint)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(firePoint, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "spreadIncrease", "morePellets" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        spreadAngle += upgrade.GetUpgradeValue("spreadIncrease");
        pellets += (int)upgrade.GetUpgradeValue("morePellets");
    }

    public override WeaponBase Clone()
    {
        ShotgunWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.spreadAngle = this.spreadAngle;
        copy.pellets = this.pellets;
        return copy;
    }
}

