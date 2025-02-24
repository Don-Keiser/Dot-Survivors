using UnityEngine;

public abstract class AreaWeapon : WeaponBase
{
    public float range;
    public GameObject areaEffectPrefab;
    protected float nextUseTime;

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        if (Time.time >= nextUseTime)
        {
            Vector2 randomPoint = (Vector2)player.position + Random.insideUnitCircle * range;
            Instantiate(areaEffectPrefab, randomPoint, Quaternion.identity);
            nextUseTime = Time.time + cooldown;
        }
    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "rangeIncrease" };
    }

        protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        damage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        range += upgrade.GetUpgradeValue("rangeIncrease");
    }
}