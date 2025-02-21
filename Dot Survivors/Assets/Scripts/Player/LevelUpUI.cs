using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button upgradeWeaponButton;
    public Button acquireWeaponButton;
    public TMP_Text upgradeWeaponText;
    public TMP_Text acquireWeaponText;

    private PlayerWeaponManager weaponManager;
    private WeaponBase weaponToUpgrade;
    private WeaponBase weaponToAcquire;

    public void Initialize(PlayerWeaponManager manager, WeaponBase upgradeWeapon, WeaponBase acquireWeapon)
    {
        weaponManager = manager;
        weaponToUpgrade = upgradeWeapon;
        weaponToAcquire = acquireWeapon;

        panel.SetActive(true); // Show UI

        upgradeWeaponText.text = weaponToUpgrade != null ? 
            $"Upgrade {weaponToUpgrade.weaponName}" : "No Weapons to Upgrade";
        acquireWeaponText.text = weaponToAcquire != null ? 
            $"Acquire {weaponToAcquire.weaponName}" : "No New Weapons Available";

        upgradeWeaponButton.interactable = weaponToUpgrade != null;
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
        Time.timeScale = 1f;
    }
}
