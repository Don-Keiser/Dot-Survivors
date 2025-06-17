using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GrenadeWeapon", menuName = "ScriptableObjects/GrenadeWeapon", order = 5)]
public class GrenadeWeapon : WeaponBase
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float grenadeSpeed = 5f;
    [SerializeField] private int grenadeCount = 1; // Start with 1 grenade
    private float cooldownTimer = 0f;
    private int lastThrownHour = 0; // Track last thrown hour

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = cooldown;
            ThrowGrenades(firePoint);
        }
    }

    private void ThrowGrenades(Transform firePoint)
    {
        List<int> grenadeHours = GetGrenadeHours();
        
        foreach (int hour in grenadeHours)
        {
            Vector2 targetPoint = GetClockPosition(firePoint.position, hour);
            GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
            grenade.GetComponent<Grenade>().Initialize(targetPoint, explosionRadius, GetModifiedDamage(), grenadeSpeed);
        }
    }

    private List<int> GetGrenadeHours()
    {
        List<int> hours = new List<int>();

        // Next hour in the clock cycle (wraps around 1 to 12)
        lastThrownHour = (lastThrownHour % 12) + 1;
        hours.Add(lastThrownHour);

        // Add extra grenades in the mirrored backward pattern
        for (int i = 1; i < grenadeCount; i++)
        {
            int mirroredHour = lastThrownHour - i;
            if (mirroredHour <= 0) mirroredHour += 12; // Wrap around
            hours.Add(mirroredHour);
        }

        return hours;
    }

    private Vector2 GetClockPosition(Vector2 center, int hour)
    {
        float angle = (hour - 3) * -30f; // Convert hour to degrees (f.e.: 12 = top, 3 = right)
        float radian = angle * Mathf.Deg2Rad;
        return center + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * throwRange;
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "explosionRadiusIncrease", "extraGrenades" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        explosionRadius += upgrade.GetUpgradeValue("explosionRadiusIncrease");
        grenadeCount += (int)upgrade.GetUpgradeValue("extraGrenades");
    }

    public override WeaponBase Clone()
    {
        GrenadeWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.explosionRadius = this.explosionRadius;
        copy.grenadeCount = this.grenadeCount;
        return copy;
    }
}