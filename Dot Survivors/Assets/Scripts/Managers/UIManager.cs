using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider xpSlider;
    [SerializeField] TMP_Text levelText;

    [SerializeField] PlayerStats playerStats;

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthBar;
            playerStats.OnXPChanged += UpdateXPBar;
            playerStats.OnLevelUp += UpdateLevelText;
        }

        // Initialize the UI
        UpdateHealthBar(playerStats.currentHealth, playerStats.maxHealth);
        UpdateXPBar(playerStats.experiencePoints, playerStats.experienceToNextLevel);
        UpdateLevelText(playerStats.level);
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }

    void UpdateXPBar(int currentXP, int xpToNextLevel)
    {
        xpSlider.value = (float)currentXP / xpToNextLevel;
    }

    void UpdateLevelText(int level)
    {
        levelText.text = "Level: " + level;
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= UpdateHealthBar;
            playerStats.OnXPChanged -= UpdateXPBar;
            playerStats.OnLevelUp -= UpdateLevelText;
        }
    }
}
