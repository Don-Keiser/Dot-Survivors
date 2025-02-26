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
    private float afterimageCooldown = Mathf.Infinity;
    private float afterimageDuration = 0.5f;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;

    [SerializeField] SpriteRenderer playerSprite;

    [Header("Iron Core Visuals")]
    public SpriteRenderer ironCoreRenderer;
    public Sprite[] ironCoreSprites;

    [Header("Battle Hardened Visuals")]
    public Sprite[] battleHardenedSprites;

    [Header("Evasive Instinct Visuals")]
    public GameObject afterimagePrefab;

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

    private void Update()
    {
        if (moveSpeedMultiplier > 1f) 
        {
            afterimageCooldown -= Time.deltaTime;
            if (afterimageCooldown <= 0f)
            {
                SpawnAfterimage();
                afterimageCooldown = afterimageDuration; // Correct cooldown reset
            }
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
                ApplyIronCore(upgradeValue, level); 
                break;
            case PassiveType.RegenerativeShell:
                ApplyRegenerativeShell(upgradeValue);
                break;
            case PassiveType.BerserkerRage:
                ApplyBerserkerRage(upgradeValue);
                break;
            case PassiveType.EvasiveInstinct:
                ApplyEvasiveInstinct(upgradeValue, level);
                break;
            case PassiveType.SharpenedReflexes:
                ApplySharpenedReflexes(upgradeValue);
                break;
            case PassiveType.BattleHardened:
                ApplyBattleHardened(passive, level);
                break;
        }
    }

    private void ApplyIronCore(float upgradeValue, int level)
    {
        maxHealthMultiplier = upgradeValue;
        playerStats.SetMaxHealth(Mathf.RoundToInt(playerStats.baseMaxHealth * maxHealthMultiplier));

        if (ironCoreRenderer != null && ironCoreSprites.Length > level -1) 
        {
            ironCoreRenderer.sprite = ironCoreSprites[level - 1];
            ironCoreRenderer.enabled = true;
        }
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

    private void ApplyEvasiveInstinct(float upgradeValue, int level)
    {
        moveSpeedMultiplier = upgradeValue;
        playerMovement.SetMoveSpeed(playerMovement.baseMoveSpeed * moveSpeedMultiplier);

        // Afterimage duration decreases more at higher levels
        afterimageDuration = Mathf.Max(0.5f / (moveSpeedMultiplier * level), 0.1f);
        afterimageCooldown = afterimageDuration; // Maintain consistent frequency
    }

    private void ApplySharpenedReflexes(float upgradeValue)
    {
        projectileSpeedMultiplier = upgradeValue;
    }

    private void ApplyBattleHardened(PassiveUpgrade passive, int level)
    {
        damageResistance = 1 * (passive.baseValue - (passive.increasePerLevel * (level - 1)));

        if (playerSprite != null && battleHardenedSprites.Length > level -1) 
        {
            playerSprite.sprite = battleHardenedSprites[level - 1];
        }
    }

    private void SpawnAfterimage()
    {
        if (afterimagePrefab == null || playerSprite == null) return;

        GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
        AfterImage afterimageScript = afterimage.GetComponent<AfterImage>();

        if (afterimageScript != null)
        {
            afterimageScript.Initialize(
                playerSprite.sprite, 
                ironCoreRenderer != null ? ironCoreRenderer.sprite : null, 
                ironCoreRenderer != null && ironCoreRenderer.enabled
            );
        }

        Destroy(afterimage, 0.5f);
    }

    public float GetDamageMultiplier() => globalDamageMultiplier;
    public float GetProjectileSpeedMultiplier() => projectileSpeedMultiplier;
    public float GetDamageResistance() => damageResistance;
}