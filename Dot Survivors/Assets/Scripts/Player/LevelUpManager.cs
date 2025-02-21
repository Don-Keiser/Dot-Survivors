using UnityEngine;
using System.Linq;

public class LevelUpManager : MonoBehaviour
{
    public PlayerWeaponManager weaponManager;
    public WeaponBase[] availableWeapons;
    public LevelUpUI levelUpUI;

    private void Start()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.OnLevelUp += HandleLevelUp;
    }

    void HandleLevelUp(int level)
    {
        Time.timeScale = 0f;

        WeaponBase weaponToUpgrade = weaponManager.weapons.Count > 0 ? 
            weaponManager.weapons[Random.Range(0, weaponManager.weapons.Count)] : null;

        WeaponBase weaponToAcquire = availableWeapons
            .Where(w => !weaponManager.weapons.Any(acquired => acquired.weaponName == w.weaponName))
            .OrderBy(_ => Random.value)
            .FirstOrDefault();

        levelUpUI.Initialize(weaponManager, weaponToUpgrade, weaponToAcquire);
    }
}
