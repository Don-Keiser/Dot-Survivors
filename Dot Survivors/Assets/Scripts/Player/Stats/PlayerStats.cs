using UnityEngine;
using System;

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

        if (currentHealth <= 0)
        {
            Die();
        }
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
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.5f);

        OnLevelUp?.Invoke(level);
        OnXPChanged?.Invoke(experiencePoints, experienceToNextLevel);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
