using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PistolWeapon", menuName = "ScriptableObjects/PistolWeapon", order = 2)]
public class PistolWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int bulletCount = 2;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletSpacing = 0.3f; 
    [SerializeField] private float rowSpawnDelay = 0.1f; // Delay between row spawning (adjustable)

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.StartCoroutine(FireBulletsCoroutine(firePoint, player));
            }
            cooldownTimer = cooldown;
        }
    }

    private IEnumerator FireBulletsCoroutine(Transform firePoint, Transform player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null) yield break;

        Vector2 fireDirection = GetCardinalDirection(playerMovement.lastMovementDirection);
        Vector2 perpendicularOffset = new Vector2(-fireDirection.y, fireDirection.x); // 90-degree offset

        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        int bulletsPerRow = Mathf.Min(4, bulletCount);
        int totalRows = Mathf.CeilToInt((float)bulletCount / 4);

        for (int row = 0; row < totalRows; row++)
        {
            int bulletsInThisRow = Mathf.Min(4, bulletCount - (row * 4));
            Vector2 rowStartPos = firePoint.position;

            for (int i = 0; i < bulletsInThisRow; i++)
            {
                float offsetAmount = (i - (bulletsInThisRow - 1) / 2f) * bulletSpacing;
                Vector2 spawnPosition = rowStartPos + perpendicularOffset * offsetAmount;

                GameObject bullet = Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = fireDirection * bulletSpeed;
                bullet.GetComponent<Projectile>().damage = GetModifiedDamage();
            }

            yield return new WaitForSeconds(rowSpawnDelay); // Delay before spawning the next row
        }
    }

    private Vector2 GetCardinalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
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
        PistolWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.bulletCount = this.bulletCount;
        return copy;
    }
}