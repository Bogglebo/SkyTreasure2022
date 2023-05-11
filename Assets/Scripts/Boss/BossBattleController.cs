using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleController : MonoBehaviour
{
    // Variable to hold boss health values
    public int maxHealth;
    private int currentHealth;
    public Slider healthSlider;

    public GameObject activateExitPortal;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageBoss()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            currentHealth = 0;
            activateExitPortal.SetActive(true);
        }
        healthSlider.value = currentHealth;

    }
}
