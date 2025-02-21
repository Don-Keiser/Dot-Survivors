using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public int level = 1;
    public float damage;
    public float cooldown;

    public abstract void UseWeapon(Transform firePoint, Transform player);
    public abstract void UpgradeWeapon();

    public virtual WeaponBase Clone()
    {
        return Instantiate(this);
    }
}
