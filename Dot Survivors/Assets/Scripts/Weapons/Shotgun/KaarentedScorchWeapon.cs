using UnityEngine;

[CreateAssetMenu(fileName = "KaarentedsScorchWeapon", menuName = "ScriptableObjects/KaarentedScorchWeapon", order = 10)]
public class KaarentedScorchWeapon : WeaponBase
{
    [SerializeField] GameObject piercingPelletPrefab;
    [SerializeField] int pellets = 5;
    [SerializeField] float spreadAngle = 15f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float burnDamage = 2f;
    [SerializeField] float burnDuration = 3f;

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
        if (closestEnemy == null) return;

        Vector2 direction = (closestEnemy.transform.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pellets; i++)
        {
            float pelletOffset = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Quaternion rot = Quaternion.Euler(0, 0, baseAngle + pelletOffset);

            GameObject pellet = Instantiate(piercingPelletPrefab, firePoint.position, rot);
            var rb = pellet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = pellet.transform.right * (bulletSpeed * PlayerPassives.Instance.GetProjectileSpeedMultiplier());

            var proj = pellet.GetComponent<Projectile>();
            proj.damage = GetModifiedDamage();
            proj.isPiercing = true;

            var burn = pellet.AddComponent<ApplyBurnOnHit>();
            burn.burnDamage = burnDamage;
            burn.burnDuration = burnDuration;
        }
    }

    private GameObject FindClosestEnemy(Vector2 origin)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(origin, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new[] { "damageIncrease", "cooldownReduction", "spreadIncrease", "morePellets", "burnDamageIncrease", "burnDurationIncrease" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        spreadAngle += upgrade.GetUpgradeValue("spreadIncrease");
        pellets += (int)upgrade.GetUpgradeValue("morePellets");
        burnDamage += upgrade.GetUpgradeValue("burnDamageIncrease");
        burnDuration += upgrade.GetUpgradeValue("burnDurationIncrease");
    }

    public override WeaponBase Clone()
    {
        var copy = Instantiate(this);
        copy.level = level;
        copy.baseDamage = baseDamage;
        copy.cooldown = cooldown;
        copy.spreadAngle = spreadAngle;
        copy.pellets = pellets;
        copy.burnDamage = burnDamage;
        copy.burnDuration = burnDuration;
        return copy;
    }
}
