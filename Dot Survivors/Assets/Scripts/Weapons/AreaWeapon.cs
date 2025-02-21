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

    public override void UpgradeWeapon()
    {
        if (!CanUpgrade()) return;

        WeaponUpgradeStep upgrade = upgradeSteps[level - 1];
        damage += upgrade.damageIncrease;
        cooldown -= upgrade.cooldownReduction;
        range += upgrade.rangeIncrease;
        level++;

        Debug.Log($"{weaponName} upgraded to Level {level}");
    }
}