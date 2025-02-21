using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponBase> weapons = new List<WeaponBase>();
    public int maxWeapons = 3;
    public Transform firePoint;
    private HashSet<string> acquiredWeaponNames = new HashSet<string>();

    public WeaponHotbarUI hotbarUI;

    private void Start()
    {
        hotbarUI.InitializeHotbar(maxWeapons); // Initialize with correct max slots

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
        hotbarUI.UpdateHotbar(weapons);
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

        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Weapon limit reached!");
            return;
        }

        WeaponBase weaponInstance = newWeapon.Clone();
        weapons.Add(weaponInstance);
        acquiredWeaponNames.Add(weaponInstance.weaponName);

        if (weaponInstance is EMPFieldWeapon empFieldWeapon)
        {
            empFieldWeapon.Activate(gameObject);
        }

        hotbarUI.UpdateHotbar(weapons);
    }

    public void UpgradeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Count)
        {
            weapons[weaponIndex].UpgradeWeapon();
        }
    }

    public void RemoveWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Count)
        {
            acquiredWeaponNames.Remove(weapons[weaponIndex].weaponName);
            weapons.RemoveAt(weaponIndex);
            hotbarUI.UpdateHotbar(weapons);
        }
    }
}
