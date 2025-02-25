using UnityEngine;
using System.Collections.Generic;

public class PlayerPassives : MonoBehaviour
{
    public static PlayerPassives Instance { get; private set; }

    private Dictionary<PassiveType, PassiveUpgrade> passives = new Dictionary<PassiveType, PassiveUpgrade>();

    private float maxHealthMultiplier = 1f;
    private float regenRate = 0f;
    private float globalDamageMultiplier = 1f;
    private float moveSpeedMultiplier = 1f;
    private float projectileSpeedMultiplier = 1f;
    private float damageResistance = 1f;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();

        if (playerStats == null || playerMovement == null)
        {
            Debug.LogError("PlayerStats or PlayerMovement component not found!");
            enabled = false;
        }
    }

    public void ApplyPassiveUpgrade(PassiveUpgrade passive)
    {
        if (!passives.ContainsKey(passive.passiveType))
        {
            passives[passive.passiveType] = passive;
        }
        else
        {
            passives[passive.passiveType].level = passive.level;
        }

        int level = passives[passive.passiveType].level;
        float upgradeValue = passive.baseValue + (passive.increasePerLevel * (level - 1));

        switch (passive.passiveType)
        {
            case PassiveType.IronCore:
                ApplyIronCore(upgradeValue); 
                break;
            case PassiveType.RegenerativeShell:
                ApplyRegenerativeShell(upgradeValue);
                break;
            case PassiveType.BerserkerRage:
                ApplyBerserkerRage(upgradeValue);
                break;
            case PassiveType.EvasiveInstinct:
                ApplyEvasiveInstinct(upgradeValue);
                break;
            case PassiveType.SharpenedReflexes:
                ApplySharpenedReflexes(upgradeValue);
                break;
            case PassiveType.BattleHardened:
                ApplyBattleHardened(passive, upgradeValue, level);
                break;
        }
    }

    private void ApplyIronCore(float upgradeValue)
    {
        maxHealthMultiplier = upgradeValue;
        playerStats.SetMaxHealth(Mathf.RoundToInt(playerStats.baseMaxHealth * maxHealthMultiplier));
    }

    private void ApplyRegenerativeShell(float upgradeValue)
    {
        regenRate = upgradeValue;
        playerStats.SetRegenRate(regenRate);
    }

    private void ApplyBerserkerRage(float upgradeValue)
    {
        globalDamageMultiplier = upgradeValue;
    }

    private void ApplyEvasiveInstinct(float upgradeValue)
    {
        moveSpeedMultiplier = upgradeValue;
        playerMovement.SetMoveSpeed(playerMovement.baseMoveSpeed * moveSpeedMultiplier);
    }

    private void ApplySharpenedReflexes(float upgradeValue)
    {
        projectileSpeedMultiplier = upgradeValue;
    }

    private void ApplyBattleHardened(PassiveUpgrade passive, float upgradeValue, int level)
    {
        damageResistance = 1 * (passive.baseValue - (passive.increasePerLevel * (level - 1)));
    }

    public float GetDamageMultiplier() => globalDamageMultiplier;
    public float GetProjectileSpeedMultiplier() => projectileSpeedMultiplier;
    public float GetDamageResistance() => damageResistance;
}