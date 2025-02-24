using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;

    public int level = 1;
    public int maxLevel = 5;

    public float damage;
    public float cooldown;

    [SerializeField]
    public WeaponUpgradeStep[] upgradeSteps;

    private void OnValidate()
    {
        if (upgradeSteps == null || upgradeSteps.Length == 0)
        {
            InitializeUpgrades();
        }
    }

    private void InitializeUpgrades()
    {
        string[] possibleStats = GetPossibleUpgradeStats();
        upgradeSteps = new WeaponUpgradeStep[maxLevel - 1];

        for (int i = 0; i < upgradeSteps.Length; i++)
        {
            upgradeSteps[i] = new WeaponUpgradeStep(possibleStats);
        }
    }

    protected abstract string[] GetPossibleUpgradeStats();

    public abstract void UseWeapon(Transform firePoint, Transform player);

    public virtual bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public virtual void UpgradeWeapon()
    {
        if (!CanUpgrade()) return;

        WeaponUpgradeStep upgrade = upgradeSteps[level - 1];
        ApplyUpgrade(upgrade);
        level++;

        Debug.Log($"{weaponName} upgraded to Level {level}");
    }
    protected abstract void ApplyUpgrade(WeaponUpgradeStep upgrade);

    public virtual WeaponBase Clone()
    {
        return Instantiate(this);
    }
}
