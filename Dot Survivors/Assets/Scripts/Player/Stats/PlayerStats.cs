using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int baseMaxHealth = 100;
    public int maxHealth;
    public int currentHealth { get; private set; }
    [SerializeField] float regenRate = 0f;

    [Header("Experience")]
    public int level { get; private set; }
    public int experiencePoints { get; private set; }
    public int experienceToNextLevel = 100;

    [Header("Movement")]
    public float baseMoveSpeed = 5f;
    private float moveSpeed;

    // Events
    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    [Header("Visuals")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject bloodParticlePrefab;
    [SerializeField] Color damageColor;

    private int regenShellLevel = 0;

    // Constants
    private static readonly float FlashDuration = 0.1f;
    private static readonly float FadeDuration = 0.3f;
    private static readonly int BloodParticleCount = 8;

    private void Awake()
    {
        maxHealth = baseMaxHealth;
        currentHealth = maxHealth;
        moveSpeed = baseMoveSpeed;
        level = 1;
        experiencePoints = 0;

        StartCoroutine(HealOverTime());
    }

    public void ApplyPassiveEffects(float maxHealthMultiplier, float newRegenRate, float moveSpeedMultiplier)
    {
        maxHealth = Mathf.RoundToInt(baseMaxHealth * maxHealthMultiplier);
        regenRate = newRegenRate;
        moveSpeed = baseMoveSpeed * moveSpeedMultiplier;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void GainXP(int xp)
    {
        experiencePoints += xp;
        OnXPChanged?.Invoke(experiencePoints, experienceToNextLevel);

        if (experiencePoints >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= Mathf.RoundToInt(damage * PlayerPassives.Instance.GetDamageResistance());
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        CameraShake.Instance?.Shake(0.2f, 0.15f);
        StartCoroutine(FlashRed());
        SpawnBloodParticles();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = Color.white;

            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(FlashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private void SpawnBloodParticles()
    {
        if (bloodParticlePrefab != null)
        {
            for (int i = 0; i < BloodParticleCount; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 0.2f;
                GameObject blood = Instantiate(bloodParticlePrefab, spawnPos, Quaternion.identity);
                Rigidbody2D rb = blood.GetComponent<Rigidbody2D>();

                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                rb.linearVelocity = randomDirection * UnityEngine.Random.Range(1.5f, 5f);

                StartCoroutine(FadeOutAndDestroy(blood));
            }
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;

        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, elapsedTime / FadeDuration);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(obj);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + Mathf.RoundToInt(amount), maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator HealOverTime()
    {
        while (true)
        {
            if (regenRate > 0)
            {
                Heal(regenRate);
                PlayerPassives.Instance.TriggerHealingAura(regenShellLevel);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetRegenShellLevel(int level)
    {
        regenShellLevel = level;
    }

    private void LevelUp()
    {
        level++;
        experiencePoints -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.25f);

        OnLevelUp?.Invoke(level);
        OnXPChanged?.Invoke(experiencePoints, experienceToNextLevel);
    }

    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetRegenRate(float newRegen)
    {
        regenRate = newRegen;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
