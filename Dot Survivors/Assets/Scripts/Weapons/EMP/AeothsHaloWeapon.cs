using UnityEngine;

[CreateAssetMenu(fileName = "AeothsHaloWeapon", menuName = "ScriptableObjects/AeothsHaloWeapon", order = 3)]
public class AeothsHaloWeapon : AreaWeapon
{
    public GameObject haloInstance;
    public float healingMultiplier = 0.2f;

    public void Activate(GameObject user)
    {
        if (haloInstance == null)
        {
            haloInstance = Instantiate(areaEffectPrefab, user.transform.position, Quaternion.identity);
            haloInstance.transform.SetParent(user.transform);

            var haloAura = haloInstance.GetComponent<AeothsHaloAura>();
            haloAura.damage = GetModifiedDamage();
            haloAura.cooldown = cooldown;
            haloAura.SetRange(range);
            haloAura.healingMultiplier = healingMultiplier;
        }
    }

    public override void UseWeapon(Transform firePoint, Transform player)
    {

    }

    protected override string[] GetPossibleUpgradeStats()
    {
        return new string[] { "damageIncrease", "cooldownReduction", "rangeIncrease", "healingMultiplierIncrease" };
    }

    protected override void ApplyUpgrade(WeaponUpgradeStep upgrade)
    {
        baseDamage += upgrade.GetUpgradeValue("damageIncrease");
        cooldown -= upgrade.GetUpgradeValue("cooldownReduction");
        range += upgrade.GetUpgradeValue("rangeIncrease");
        healingMultiplier += upgrade.GetUpgradeValue("healingMultiplierIncrease");

        if (haloInstance != null)
        {
            AeothsHaloAura haloAura = haloInstance.GetComponent<AeothsHaloAura>();
            haloAura.damage = GetModifiedDamage();
            haloAura.cooldown = cooldown;
            haloAura.SetRange(range);

        }
    }

    public override WeaponBase Clone()
    {
        AeothsHaloWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.baseDamage = this.baseDamage;
        copy.cooldown = this.cooldown;
        copy.range = this.range;
        copy.healingMultiplier = this.healingMultiplier;
        return copy;
    }
}
