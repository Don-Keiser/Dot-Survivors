using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponBase> weapons = new List<WeaponBase>();
    private HashSet<string> acquiredWeaponNames = new HashSet<string>();
    public Transform firePoint;

    private void Start()
    {
        List<WeaponBase> clonedWeapons = new List<WeaponBase>();
        foreach (var weapon in weapons)
        {
            WeaponBase weaponInstance = weapon.Clone();
            clonedWeapons.Add(weaponInstance);
            acquiredWeaponNames.Add(weaponInstance.weaponName);

            if (weaponInstance is EMPFieldWeapon empFieldWeapon)
            {
                empFieldWeapon.Activate(gameObject);
            }
        }
        weapons = clonedWeapons;
    }

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            weapon.UseWeapon(firePoint, transform);
        }
    }

    public void AddWeapon(WeaponBase newWeapon)
    {
        if (acquiredWeaponNames.Contains(newWeapon.weaponName))
        {
            Debug.Log($"Weapon {newWeapon.weaponName} is already acquired!");
            return;
        }

        WeaponBase weaponInstance = newWeapon.Clone();
        weapons.Add(weaponInstance);
        acquiredWeaponNames.Add(weaponInstance.weaponName);

        if (weaponInstance is EMPFieldWeapon empFieldWeapon)
        {
            empFieldWeapon.Activate(gameObject);
        }
    }

    public void UpgradeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Count)
        {
            weapons[weaponIndex].UpgradeWeapon();
        }
    }
}
