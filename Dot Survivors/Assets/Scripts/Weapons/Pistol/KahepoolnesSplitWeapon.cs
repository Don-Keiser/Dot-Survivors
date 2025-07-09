using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "KahepoolnesSplitWeapon", menuName = "ScriptableObjects/KahepoolnesSplitWeapon", order = 10)]
public class KahepoolnesSplitWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int bulletCount = 2;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletSpacing = 0.3f;
    [SerializeField] private float rowSpawnDelay = 0.1f;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.StartCoroutine(FireBulletsBothWays(firePoint, player));

            cooldownTimer = cooldown;
        }
    }

    private IEnumerator FireBulletsBothWays(Transform firePoint, Transform player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null) yield break;

        Vector2 forwardDir = GetCardinalDirection(playerMovement.lastMovementDirection);
        Vector2 backwardDir = -forwardDir;
        float forwardAngle = Mathf.Atan2(forwardDir.y, forwardDir.x) * Mathf.Rad2Deg;
        float backwardAngle = Mathf.Atan2(backwardDir.y, backwardDir.x) * Mathf.Rad2Deg;
        Vector2 offsetDir = new Vector2(-forwardDir.y, forwardDir.x);

        int bulletsPerRow = Mathf.Min(4, bulletCount);
        int totalRows = Mathf.CeilToInt((float)bulletCount / 4);

        for (int row = 0; row < totalRows; row++)
        {
            int bulletsInThisRow = Mathf.Min(4, bulletCount - (row * 4));

            for (int i = 0; i < bulletsInThisRow; i++)
            {
                float offset = (i - (bulletsInThisRow - 1) / 2f) * bulletSpacing;
                Vector2 spawnPos = (Vector2)firePoint.position + offsetDir * offset;

                // Forward bullet
                GameObject bulletF = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, forwardAngle));
                bulletF.GetComponent<Rigidbody2D>().linearVelocity = forwardDir * (bulletSpeed * PlayerPassives.Instance.GetProjectileSpeedMultiplier());
                bulletF.GetComponent<Projectile>().damage = GetModifiedDamage();

                // Backward bullet
                GameObject bulletB = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, backwardAngle));
                bulletB.GetComponent<Rigidbody2D>().linearVelocity = backwardDir * (bulletSpeed * PlayerPassives.Instance.GetProjectileSpeedMultiplier());
                bulletB.GetComponent<Projectile>().damage = GetModifiedDamage();
            }

            yield return new WaitForSeconds(rowSpawnDelay);
        }
    }

    private Vector2 GetCardinalDirection(Vector2 dir)
    {
        dir.Normalize();
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) return dir.x > 0 ? Vector2.right : Vector2.left;
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x)) return dir.y > 0 ? Vector2.up : Vector2.down;
        if (dir.x > 0 && dir.y > 0) return new Vector2(1, 1).normalized;
        if (dir.x < 0 && dir.y > 0) return new Vector2(-1, 1).normalized;
        if (dir.x > 0 && dir.y < 0) return new Vector2(1, -1).normalized;
        if (dir.x < 0 && dir.y < 0) return new Vector2(-1, -1).normalized;
        return Vector2.zero;
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "extraBullets" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        bulletCount += (int)upgrade.GetUpgradeValue("extraBullets");
    }

    public override WeaponBase Clone()
    {
        KahepoolnesSplitWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.bulletCount = this.bulletCount;
        return copy;
    }
}