using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;

    [Header("Weapon UI Elements")]
    public Button upgradeWeaponButton;
    public Button acquireWeaponButton;
    public TMP_Text upgradeWeaponText;
    public TMP_Text acquireWeaponText;
    public Image upgradeWeaponIcon;
    public Image acquireWeaponIcon;

    [Header("Passive UI Elements")]
    public Button upgradePassiveButton;
    public Button acquirePassiveButton;
    public TMP_Text upgradePassiveText;
    public TMP_Text acquirePassiveText;
    public Image upgradePassiveIcon;
    public Image acquirePassiveIcon;

    // Private fields
    private System.Action onLevelUpComplete;
    private PlayerWeaponManager weaponManager;
    private PlayerPassiveManager passiveManager;
    private WeaponBase weaponToUpgrade;
    private WeaponBase weaponToAcquire;
    private PassiveUpgrade passiveToUpgrade;
    private PassiveUpgrade passiveToAcquire;

    private const string NoWeaponsToUpgrade = "No Weapons to Upgrade";
    private const string NoNewWeaponsAvailable = "No New Weapons Available";
    private const string NoPassivesToUpgrade = "No Passives to Upgrade";
    private const string NoNewPassivesAvailable = "No New Passives Available";

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

        HandleWeaponUpgrade();
        HandleWeaponAcquisition();
        HandlePassiveUpgrade();
        HandlePassiveAcquisition();
    }

    private void HandleWeaponUpgrade()
    {
        if (weaponToUpgrade != null)
        {
            upgradeWeaponText.text = weaponToUpgrade.level < weaponToUpgrade.maxLevel
                ? $"Upgrade {weaponToUpgrade.weaponName} (Level {weaponToUpgrade.level}/{weaponToUpgrade.maxLevel})"
                : $"{weaponToUpgrade.weaponName} (Max Level)";

            upgradeWeaponIcon.sprite = weaponToUpgrade.weaponIcon;
            upgradeWeaponButton.interactable = weaponToUpgrade.level < weaponToUpgrade.maxLevel;
        }
        else
        {
            upgradeWeaponText.text = NoWeaponsToUpgrade;
            upgradeWeaponButton.interactable = false;
        }
    }

    private void HandleWeaponAcquisition()
    {
        acquireWeaponText.text = weaponToAcquire != null 
            ? $"Acquire {weaponToAcquire.weaponName}" 
            : NoNewWeaponsAvailable;

        if (weaponToAcquire != null)
        {
            acquireWeaponIcon.sprite = weaponToAcquire.weaponIcon;
        }

        acquireWeaponButton.interactable = weaponToAcquire != null;
    }

    private void HandlePassiveUpgrade()
    {
        if (passiveToUpgrade != null)
        {
            upgradePassiveText.text = passiveToUpgrade.level < passiveToUpgrade.maxLevel
                ? $"Upgrade {passiveToUpgrade.passiveName} (Level {passiveToUpgrade.level}/{passiveToUpgrade.maxLevel})"
                : $"{passiveToUpgrade.passiveName} (Max Level)";

            upgradePassiveIcon.sprite = passiveToUpgrade.passiveIcon;
            upgradePassiveButton.interactable = passiveToUpgrade.level < passiveToUpgrade.maxLevel;
        }
        else
        {
            upgradePassiveText.text = NoPassivesToUpgrade;
            upgradePassiveButton.interactable = false;
        }
    }

    private void HandlePassiveAcquisition()
    {
        acquirePassiveText.text = passiveToAcquire != null 
            ? $"Acquire {passiveToAcquire.passiveName}" 
            : NoNewPassivesAvailable;

        if (passiveToAcquire != null)
        {
            acquirePassiveIcon.sprite = passiveToAcquire.passiveIcon;
        }

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