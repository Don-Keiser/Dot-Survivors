using UnityEngine;

[System.Serializable]
public class WeaponUpgradeStep
{
    public float damageIncrease;
    public float cooldownReduction;
    public float rangeIncrease;
    public int extraShots;
}

public abstract class WeaponBase : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;

    public int level = 1;
    public int maxLevel = 5;

    public float damage;
    public float cooldown;

    public WeaponUpgradeStep[] upgradeSteps;

    public abstract void UseWeapon(Transform firePoint, Transform player);

    public virtual bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public virtual void UpgradeWeapon()
    {
        if (!CanUpgrade()) return;

        WeaponUpgradeStep upgrade = upgradeSteps[level - 1];
        damage += upgrade.damageIncrease;
        cooldown -= upgrade.cooldownReduction;
        level++;

        Debug.Log($"{weaponName} upgraded to Level {level}");
    }

    public virtual WeaponBase Clone()
    {
        return Instantiate(this);
    }
}
