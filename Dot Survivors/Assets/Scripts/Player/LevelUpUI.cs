using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;

    // Weapon UI Elements
    public Button upgradeWeaponButton;
    public Button acquireWeaponButton;
    public TMP_Text upgradeWeaponText;
    public TMP_Text acquireWeaponText;

    // Passive UI Elements
    public Button upgradePassiveButton;
    public Button acquirePassiveButton;
    public TMP_Text upgradePassiveText;
    public TMP_Text acquirePassiveText;

    private System.Action onLevelUpComplete;
    private PlayerWeaponManager weaponManager;
    private PlayerPassiveManager passiveManager;
    private WeaponBase weaponToUpgrade;
    private WeaponBase weaponToAcquire;
    private PassiveUpgrade passiveToUpgrade;
    private PassiveUpgrade passiveToAcquire;

    public void Initialize(
        PlayerWeaponManager wManager, 
        PlayerPassiveManager pManager, 
        WeaponBase upgradeWeapon, 
        WeaponBase acquireWeapon, 
        PassiveUpgrade upgradePassive, 
        PassiveUpgrade acquirePassive, 
        System.Action onComplete)
    {
        weaponManager = wManager;
        passiveManager = pManager;
        weaponToUpgrade = upgradeWeapon;
        weaponToAcquire = acquireWeapon;
        passiveToUpgrade = upgradePassive;
        passiveToAcquire = acquirePassive;
        onLevelUpComplete = onComplete;

        panel.SetActive(true);

        // Handle Weapon Upgrade
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

        // Handle Weapon Acquisition
        acquireWeaponText.text = weaponToAcquire != null 
            ? $"Acquire {weaponToAcquire.weaponName}" 
            : "No New Weapons Available";

        acquireWeaponButton.interactable = weaponToAcquire != null;

        // Handle Passive Upgrade
        if (passiveToUpgrade != null)
        {
            upgradePassiveText.text = $"Upgrade {passiveToUpgrade.passiveName} (Level {passiveToUpgrade.level}/{passiveToUpgrade.maxLevel})";
            upgradePassiveButton.interactable = true;
        }
        else
        {
            upgradePassiveText.text = "No Passives to Upgrade";
            upgradePassiveButton.interactable = false;
        }

        // Handle Passive Acquisition
        acquirePassiveText.text = passiveToAcquire != null 
            ? $"Acquire {passiveToAcquire.passiveName}" 
            : "No New Passives Available";

        acquirePassiveButton.interactable = passiveToAcquire != null;
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

    public void OnUpgradePassive()
    {
        if (passiveToUpgrade != null)
        {
            passiveToUpgrade.UpgradePassive();
            PlayerPassives.Instance.ApplyPassiveUpgrade(passiveToUpgrade);
        }
        CloseMenu();
    }

    public void OnAcquirePassive()
    {
        if (passiveToAcquire != null && passiveManager != null)
        {
            passiveManager.AddPassive(passiveToAcquire);
        }
        CloseMenu();
    }

    private void CloseMenu()
    {
        panel.SetActive(false);
        onLevelUpComplete?.Invoke();
    }
}