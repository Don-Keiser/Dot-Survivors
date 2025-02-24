using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public int level { get; private set; }
    public int experiencePoints { get; private set; }
    public int experienceToNextLevel = 100;

    public float moveSpeed = 5f;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject bloodParticlePrefab;
    [SerializeField] Color damageColor;

    private void Awake()
    {
        currentHealth = maxHealth;
        level = 1;
        experiencePoints = 0;
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
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
        }
    }

    private void SpawnBloodParticles()
    {
        if (bloodParticlePrefab != null)
        {
            for (int i = 0; i < 10; i++) // Number of blood particles
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
        float fadeDuration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(obj);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void LevelUp()
    {
        level++;
        experiencePoints -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.25f);

        OnLevelUp?.Invoke(level);
        OnXPChanged?.Invoke(experiencePoints, experienceToNextLevel);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
