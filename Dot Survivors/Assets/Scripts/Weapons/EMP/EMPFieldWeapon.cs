using UnityEngine;

[CreateAssetMenu(fileName = "EMPFieldWeapon", menuName = "ScriptableObjects/EMPFieldWeapon", order = 2)]
public class EMPFieldWeapon : AreaWeapon
{
    [SerializeField] GameObject empInstance;

    public void Activate(GameObject user)
    {
        if (empInstance == null)
        {
            empInstance = Instantiate(areaEffectPrefab, user.transform.position, Quaternion.identity);
            empInstance.transform.SetParent(user.transform);

            EMPAura empAura = empInstance.GetComponent<EMPAura>();
            empAura.damage = damage;
            empAura.cooldown = cooldown;
            empAura.SetRange(range);
        }
    }

    public override void UseWeapon(Transform firePoint, Transform player)
    {
        
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

        if (empInstance != null)
        {
            EMPAura empAura = empInstance.GetComponent<EMPAura>();
            empAura.damage = damage;
            empAura.cooldown = cooldown;
            empAura.SetRange(range);
        }
    }

    public override WeaponBase Clone()
    {
        EMPFieldWeapon copy = Instantiate(this);
        copy.level = this.level;
        copy.damage = this.damage;
        copy.cooldown = this.cooldown;
        copy.range = this.range;
        return copy;
    }
}