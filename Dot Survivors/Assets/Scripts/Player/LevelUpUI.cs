using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    public Sprite noAvaibleIcon;

    [Header("Passive UI Elements")]
    public Button upgradePassiveButton;
    public Button acquirePassiveButton;
    public TMP_Text upgradePassiveText;
    public TMP_Text acquirePassiveText;
    public Image upgradePassiveIcon;
    public Image acquirePassiveIcon;

    [Header("Descriptions")]
    public TMP_Text weaponDescriptionText;
    public TMP_Text passiveDescriptionText;
    public TMP_Text weaponADescriptionText;
    public TMP_Text passiveADescriptionText;

    [Header("Synergy Popup")]
    [SerializeField] private GameObject synergyPanel;
    [SerializeField] private TMP_Text synergyNameText;
    [SerializeField] private TMP_Text synergyDescriptionText;
    [SerializeField] private Image synergyIcon;

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
    
    private bool isSynergyPopupActive = false;

    //PopUp anim
    private Coroutine synergyAnimCoroutine;
    private float popupAnimDuration = 0.25f;

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
            weaponDescriptionText.text = weaponToUpgrade.description;
            upgradeWeaponButton.interactable = weaponToUpgrade.level < weaponToUpgrade.maxLevel;
        }
        else
        {
            upgradeWeaponText.text = NoWeaponsToUpgrade;
            upgradeWeaponButton.interactable = false;
            upgradeWeaponIcon.sprite = noAvaibleIcon;
            weaponDescriptionText.text = "";
        }
    }

    private void HandleWeaponAcquisition()
    {
        if (weaponToAcquire != null)
        {
            acquireWeaponText.text = $"Acquire {weaponToAcquire.weaponName}";
            acquireWeaponIcon.sprite = weaponToAcquire.weaponIcon;
            weaponADescriptionText.text = weaponToAcquire.description;
        }
        else
        {
            acquireWeaponText.text = NoNewWeaponsAvailable;
            acquireWeaponIcon.sprite = noAvaibleIcon;
            weaponADescriptionText.text = "";
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
            passiveDescriptionText.text = passiveToUpgrade.description;
            upgradePassiveButton.interactable = passiveToUpgrade.level < passiveToUpgrade.maxLevel;
        }
        else
        {
            upgradePassiveText.text = NoPassivesToUpgrade;
            upgradePassiveIcon.sprite = noAvaibleIcon;
            passiveDescriptionText.text = "";
            upgradePassiveButton.interactable = false;
        }
    }

    private void HandlePassiveAcquisition()
    {
        if (passiveToAcquire != null)
        {
            acquirePassiveText.text = $"Acquire {passiveToAcquire.passiveName}";
            acquirePassiveIcon.sprite = passiveToAcquire.passiveIcon;
            passiveADescriptionText.text = passiveToAcquire.description;
        }
        else
        {
            acquirePassiveText.text = NoNewPassivesAvailable;
            acquirePassiveIcon.sprite = noAvaibleIcon;
            passiveADescriptionText.text = "";
        }

        acquirePassiveButton.interactable = passiveToAcquire != null;
    }

    public void OnUpgradeWeapon()
    {
        if (weaponToUpgrade != null)
        {
            weaponToUpgrade.UpgradeWeapon();
            SynergyManager.Instance?.TryCheckSynergies();
        }

        if (!isSynergyPopupActive)
            CloseMenu();
    }

    public void OnAcquireWeapon()
    {
        if (weaponToAcquire != null && weaponManager != null)
            weaponManager.AddWeapon(weaponToAcquire);

        if (!isSynergyPopupActive)
            CloseMenu();
    }

    public void OnUpgradePassive()
    {
        if (passiveToUpgrade != null)
        {
            passiveToUpgrade.UpgradePassive();
            PlayerPassives.Instance.ApplyPassiveUpgrade(passiveToUpgrade);
            SynergyManager.Instance?.TryCheckSynergies();
        }

        if (!isSynergyPopupActive)
            CloseMenu();
    }

    public void OnAcquirePassive()
    {
        if (passiveToAcquire != null && passiveManager != null)
            passiveManager.AddPassive(passiveToAcquire);

        if (!isSynergyPopupActive)
            CloseMenu();
    }

    public void ShowSynergyPopup(WeaponSynergy synergy)
    {
        panel.SetActive(false); // Disable background UI

        synergyPanel.SetActive(true);
        synergyPanel.transform.localScale = Vector3.zero;

        synergyNameText.text = synergy.synergyName;
        synergyDescriptionText.text = synergy.synergyWeapon.description;
        synergyIcon.sprite = synergy.synergyPopUpSprite;

        if (synergyAnimCoroutine != null) StopCoroutine(synergyAnimCoroutine);
        synergyAnimCoroutine = StartCoroutine(AnimatePopup(true));

        isSynergyPopupActive = true;
    }

    public void OnSynergyPopupClicked()
    {
        SynergyManager.Instance.ConfirmSynergy();

        if (synergyAnimCoroutine != null) StopCoroutine(synergyAnimCoroutine);
        synergyAnimCoroutine = StartCoroutine(AnimatePopup(false));
    }

    private IEnumerator AnimatePopup(bool poppingIn)
    {
        float time = 0f;
        Vector3 start = poppingIn ? Vector3.zero : Vector3.one;
        Vector3 end = poppingIn ? Vector3.one : Vector3.zero;

        while (time < popupAnimDuration)
        {
            synergyPanel.transform.localScale = Vector3.Lerp(start, end, time / popupAnimDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        synergyPanel.transform.localScale = end;

        if (!poppingIn)
        {
            isSynergyPopupActive = false;
            synergyPanel.SetActive(false);
            CloseMenu();
        }
    }

    private void CloseMenu()
    {
        panel.SetActive(false);
        onLevelUpComplete?.Invoke();
    }
}