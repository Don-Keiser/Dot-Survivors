using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSynergy
{
    public string synergyName;
    public WeaponBase baseWeapon;
    public PassiveType requiredPassive;
    public WeaponBase synergyWeapon;
    public Sprite synergyPopUpSprite;
}

public class SynergyManager : MonoBehaviour
{
    public static SynergyManager Instance { get; private set; }

    [SerializeField] private List<WeaponSynergy> synergyDefinitions;
    [SerializeField] private PlayerWeaponManager weaponManager;
    [SerializeField] private PlayerPassiveManager passiveManager;   
    [SerializeField] private LevelUpUI levelUpUI;

    private WeaponSynergy pendingSynergy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TryCheckSynergies()
    {
        foreach (var synergy in synergyDefinitions)
        {
            bool hasBaseWeapon = weaponManager.weapons.Exists(w => w.weaponName == synergy.baseWeapon.weaponName && w.level >= w.maxLevel);
            bool hasPassive = passiveManager.acquiredPassives.Exists(p => p.passiveType == synergy.requiredPassive && p.level >= p.maxLevel);
            bool alreadyHasSynergy = weaponManager.weapons.Exists(w => w.weaponName == synergy.synergyWeapon.weaponName);

            if (hasBaseWeapon && hasPassive && !alreadyHasSynergy)
            {
                pendingSynergy = synergy;
                levelUpUI.ShowSynergyPopup(synergy);
                break;
            }
        }
    }

    public void ConfirmSynergy()
    {
        if (pendingSynergy != null)
        {
            ReplaceWithSynergy(pendingSynergy);
            pendingSynergy = null;
        }
    }

    private void ReplaceWithSynergy(WeaponSynergy synergy)
    {
        int index = weaponManager.weapons.FindIndex(w => w.weaponName == synergy.baseWeapon.weaponName);
        if (index == -1) return;

        weaponManager.RemoveWeapon(index);
        weaponManager.AddWeapon(synergy.synergyWeapon);
        Debug.Log($"Synergy activated: {synergy.synergyName}");
    }
}
