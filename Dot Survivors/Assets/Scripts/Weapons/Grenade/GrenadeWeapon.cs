using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeWeapon", menuName = "ScriptableObjects/GrenadeWeapon", order = 5)]
public class GrenadeWeapon : WeaponBase
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float grenadeSpeed = 5f;

    private float cooldownTimer = 0f;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = cooldown;
            ThrowGrenade(firePoint);
        }
    }

    private void ThrowGrenade(Transform firePoint)
    {
        // Pick a random target point within the range
        Vector2 targetPoint = (Vector2)firePoint.position + Random.insideUnitCircle * throwRange;

        // Create grenade and set its target
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
        grenade.GetComponent<Grenade>().Initialize(targetPoint, explosionRadius, damage, grenadeSpeed);
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "explosionRadiusIncrease" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        damage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        explosionRadius += upgrade.GetUpgradeValue("explosionRadiusIncrease");
    }

    public override WeaponBase Clone()
    {
        GrenadeWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.damage = this.damage;
        copy.cooldown = this.cooldown;
        copy.explosionRadius = this.explosionRadius;
        return copy;
    }
}