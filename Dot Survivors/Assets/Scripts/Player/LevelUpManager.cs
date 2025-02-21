using UnityEngine;
using System.Linq;

public class LevelUpManager : MonoBehaviour
{
    public PlayerWeaponManager weaponManager;
    public WeaponBase[] availableWeapons;
    public LevelUpUI levelUpUI;

    private int pendingLevelUps = 0;
    private bool isProcessingLevelUp = false;

    private void Start()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.OnLevelUp += HandleLevelUp;
    }

    private void HandleLevelUp(int level)
    {
        pendingLevelUps++;

        if (!isProcessingLevelUp)
        {
            ProcessLevelUp();
        }
    }

    private void ProcessLevelUp()
    {
        if (pendingLevelUps <= 0)
        {
            isProcessingLevelUp = false;
            Time.timeScale = 1f;
            return;
        }

        isProcessingLevelUp = true;
        Time.timeScale = 0f;
        WeaponBase weaponToUpgrade = weaponManager.weapons.Count > 0 ?
            weaponManager.weapons[Random.Range(0, weaponManager.weapons.Count)] : null;

        WeaponBase weaponToAcquire = null;

        if (weaponManager.weapons.Count < weaponManager.maxWeapons)
        {
            weaponToAcquire = availableWeapons
                .Where(w => !weaponManager.weapons.Any(acquired => acquired.weaponName == w.weaponName))
                .OrderBy(_ => Random.value)
                .FirstOrDefault();
        }

        levelUpUI.Initialize(weaponManager, weaponToUpgrade, weaponToAcquire, OnLevelUpChoiceMade);
    }

    private void OnLevelUpChoiceMade()
    {
        pendingLevelUps--;

        if (pendingLevelUps > 0)
        {
            ProcessLevelUp();
        }
        else
        {
            Time.timeScale = 1f;
            isProcessingLevelUp = false;
        }
    }
}
