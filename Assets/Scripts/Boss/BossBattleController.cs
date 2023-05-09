using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    // Variable to hold boss health values
    public int maxHealth;
    private int currentHealth;

    public GameObject activateExitPortal;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

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
            activateExitPortal.SetActive(true);

        }
    }
}
