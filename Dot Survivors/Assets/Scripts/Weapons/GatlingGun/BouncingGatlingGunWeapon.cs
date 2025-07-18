// Scripts/Weapons/Gatling/BouncingGatlingGunWeapon.cs
using UnityEngine;

[CreateAssetMenu(fileName = "BouncingGatlingGun", menuName = "ScriptableObjects/BouncingGatlingGunWeapon")]
public class BouncingGatlingGunWeapon : WeaponBase
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float rotationSpeed = 120f;
    [SerializeField] int bounceCount = 3;
    [SerializeField] float bounceRange = 3f;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = fireRate;
            FireQuad(firePoint);
        }

        firePoint.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void FireQuad(Transform firePoint)
    {
        FireProjectile(firePoint.rotation, firePoint);
        FireProjectile(Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 90f), firePoint);
        FireProjectile(Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 180f), firePoint);
        FireProjectile(Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 270f), firePoint);
    }

    private void FireProjectile(Quaternion rotation, Transform firePoint)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotation);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = bullet.transform.right * (10f * PlayerPassives.Instance.GetProjectileSpeedMultiplier());

        var proj = bullet.GetComponent<Projectile>();
        proj.damage = GetModifiedDamage();
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "fireRateIncrease", "rotationSpeedIncrease" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        fireRate *= upgrade.GetUpgradeValue("fireRateIncrease");
        rotationSpeed += upgrade.GetUpgradeValue("rotationSpeedIncrease");
    }

    public override WeaponBase Clone()
    {
        BouncingGatlingGunWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.fireRate = this.fireRate;
        copy.rotationSpeed = this.rotationSpeed;
        return copy;
    }
}
