using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public PlayerWeaponManager weaponManager;
    public WeaponBase[] availableWeapons;

    void Start()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.OnLevelUp += HandleLevelUp;
    }

    void HandleLevelUp(int level)
    {
        if (Random.value > 0.5f)
        {
            AcquireNewWeapon();
        }
        else
        {
            UpgradeRandomWeapon();
        }
    }

    void AcquireNewWeapon()
    {
        WeaponBase newWeapon = availableWeapons[Random.Range(0, availableWeapons.Length)];
        weaponManager.AddWeapon(newWeapon);
    }

    void UpgradeRandomWeapon()
    {
        int weaponIndex = Random.Range(0, weaponManager.weapons.Count);
        weaponManager.UpgradeWeapon(weaponIndex);
    }
}