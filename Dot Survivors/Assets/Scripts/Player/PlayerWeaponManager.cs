using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponBase> weapons;
    public Transform firePoint;

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            weapon.UseWeapon(firePoint, transform);
        }
    }

    public void AddWeapon(WeaponBase newWeapon)
    {
        weapons.Add(newWeapon);
    }

    public void UpgradeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Count)
        {
            weapons[weaponIndex].UpgradeWeapon();
        }
    }
}