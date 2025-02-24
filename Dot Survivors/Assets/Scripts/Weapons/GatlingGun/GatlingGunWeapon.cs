using UnityEngine;

[CreateAssetMenu(fileName = "GatlingGunWeapon", menuName = "ScriptableObjects/GatlingGunWeapon", order = 3)]
public class GatlingGunWeapon : WeaponBase
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float rotationSpeed = 120f;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = fireRate;
            FireGatling(firePoint);
        }

        // Rotate the firing point around the player
        firePoint.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void FireGatling(Transform firePoint)
    {
        Quaternion rotation = firePoint.rotation;
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.right * 10f;
        bullet.GetComponent<Projectile>().damage = damage;
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "fireRateIncrease", "rotationSpeedIncrease" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        damage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        fireRate *= upgrade.GetUpgradeValue("fireRateIncrease");
        rotationSpeed += upgrade.GetUpgradeValue("rotationSpeedIncrease");
    }

    public override WeaponBase Clone()
    {
        GatlingGunWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.damage = this.damage;
        copy.cooldown = this.cooldown;
        copy.fireRate = this.fireRate;
        return copy;
    }
}
