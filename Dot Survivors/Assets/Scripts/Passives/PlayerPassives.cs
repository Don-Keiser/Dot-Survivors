using UnityEngine;
using System.Collections.Generic;

public class PlayerPassives : MonoBehaviour
{
    public static PlayerPassives Instance { get; private set; }

    private Dictionary<string, PassiveUpgrade> passives = new Dictionary<string, PassiveUpgrade>();

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
    }

    public void ApplyPassiveUpgrade(PassiveUpgrade passive)
    {
        if (!passives.ContainsKey(passive.passiveName))
        {
            passives[passive.passiveName] = passive;
        }
        else
        {
            passives[passive.passiveName].level = passive.level;
        }

        int level = passives[passive.passiveName].level;
        float upgradeValue = passive.baseValue + (passive.increasePerLevel * (level - 1));

        switch (passive.passiveName)
        {
            case "Iron Core":
                maxHealthMultiplier = upgradeValue;
                playerStats.SetMaxHealth(Mathf.RoundToInt(playerStats.baseMaxHealth * maxHealthMultiplier)); 
                break;
            case "Regenerative Shell":
                regenRate = upgradeValue;
                playerStats.SetRegenRate(regenRate);
                break;
            case "Berserker Rage":
                globalDamageMultiplier = upgradeValue;
                break;
            case "Evasive Instinct":
                moveSpeedMultiplier = upgradeValue;
                playerMovement.SetMoveSpeed(playerMovement.baseMoveSpeed * moveSpeedMultiplier);
                break;
            case "Sharpened Reflexes":
                projectileSpeedMultiplier = upgradeValue;
                break;
            case "Battle Hardened":
                damageResistance = 1 * (passive.baseValue - (passive.increasePerLevel * (level - 1)));
                break;
        }
    }

    public float GetDamageMultiplier() => globalDamageMultiplier;
    public float GetProjectileSpeedMultiplier() => projectileSpeedMultiplier;
    public float GetDamageResistance() => damageResistance;
}