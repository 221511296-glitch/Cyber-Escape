using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarSystem : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Text healthText;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color mediumHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;
    // [SerializeField] private float healthChangeAnimationDuration = 0.3f;

    private int maxHealth = 100;
    private int currentHealth;
    private int healthDecreasePerMistake = 20;
    private bool isInitialized = false;

    public void Initialize(int maxHealthValue, int decreasePerMistake)
    {
        maxHealth = maxHealthValue;
        currentHealth = maxHealth;
        healthDecreasePerMistake = decreasePerMistake;
        isInitialized = true;
        UpdateHealthDisplay();
    }

    public void DecreaseHealth()
    {
        if (!isInitialized) return;

        currentHealth = Mathf.Max(0, currentHealth - healthDecreasePerMistake);
        UpdateHealthDisplay();
    }

    public void IncreaseHealth(int amount)
    {
        if (!isInitialized) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;

            // Change color based on health
            if (currentHealth > maxHealth * 0.5f)
                healthBarFill.color = fullHealthColor;
            else if (currentHealth > maxHealth * 0.25f)
                healthBarFill.color = mediumHealthColor;
            else
                healthBarFill.color = lowHealthColor;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}%";
        }
    }

    public bool IsGameOver()
    {
        return currentHealth <= 0;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }
}
