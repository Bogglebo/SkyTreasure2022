using UnityEngine;

// Script to manage player health
public class HealthController : MonoBehaviour
{
    // Create a static instance of the Health Controller
    public static HealthController instance;
    // Variables to contain current and max health values
    public int currentHealth, maxHealth;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assign health value when player starts
        currentHealth = maxHealth;
        // Update the display value on the UI
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Remove health when damaged
    public void Damage()
    {
        // Reduce health by 1
        currentHealth--;
        // Play sound
        AudioController.instance.PlayFX(9);

        // Reduce the score 
        ScoreController.instance.UpdateScore(-50);
        // If the score becomes negative set it to 0 & respawn
        if (ScoreController.instance.theScore < 0)
        {
            ScoreController.instance.theScore = 0;
            ScoreController.instance.UpdateScore(0);
            SystemController.instance.Respawn();
        }

        // If player is dead
        if (currentHealth <= 0)
        {
            // Set health to 0 and respawn player
            currentHealth = 0;
            SystemController.instance.Respawn();
        }
        else
        {
            PlayerController.instance.Knockback();
        }
        // Update the display value on the UI
        UpdateUI();
    }

    // Restore health to full with powerup
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        // Update the display value on the UI
        UpdateUI();
    }

    // Update the UI health display with the current health
    public void UpdateUI()
    {
        // Get the text from the health text component
        UIController.instance.healthText.text = "Health: " + currentHealth.ToString();
    }

    // Increase health by a certain amount
    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        // Update the display value on the UI
        UpdateUI();
    }
}
