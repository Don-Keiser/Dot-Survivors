using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button upgradeWeaponButton;
    public Button acquireWeaponButton;
    public TMP_Text upgradeWeaponText;
    public TMP_Text acquireWeaponText;

    private System.Action onLevelUpComplete;
    private PlayerWeaponManager weaponManager;
    private WeaponBase weaponToUpgrade;
    private WeaponBase weaponToAcquire;

    public void Initialize(PlayerWeaponManager manager, WeaponBase upgradeWeapon, WeaponBase acquireWeapon, System.Action onComplete)
    {
        weaponManager = manager;
        weaponToUpgrade = upgradeWeapon;
        weaponToAcquire = acquireWeapon;
        onLevelUpComplete = onComplete;

        panel.SetActive(true);

        if (weaponToUpgrade != null)
        {
            upgradeWeaponText.text = weaponToUpgrade.level < weaponToUpgrade.maxLevel
                ? $"Upgrade {weaponToUpgrade.weaponName} (Level {weaponToUpgrade.level}/{weaponToUpgrade.maxLevel})"
                : $"{weaponToUpgrade.weaponName} (Max Level)";

            upgradeWeaponButton.interactable = weaponToUpgrade.level < weaponToUpgrade.maxLevel;
        }
        else
        {
            upgradeWeaponText.text = "No Weapons to Upgrade";
            upgradeWeaponButton.interactable = false;
        }

        acquireWeaponText.text = weaponToAcquire != null 
            ? $"Acquire {weaponToAcquire.weaponName}" 
            : "No New Weapons Available";

        acquireWeaponButton.interactable = weaponToAcquire != null;
    }

    public void OnUpgradeWeapon()
    {
        if (weaponToUpgrade != null)
        {
            weaponToUpgrade.UpgradeWeapon();
        }
        CloseMenu();
    }

    public void OnAcquireWeapon()
    {
        if (weaponToAcquire != null && weaponManager != null)
        {
            weaponManager.AddWeapon(weaponToAcquire);
        }
        CloseMenu();
    }

    private void CloseMenu()
    {
        panel.SetActive(false);
        onLevelUpComplete?.Invoke();
    }
}
