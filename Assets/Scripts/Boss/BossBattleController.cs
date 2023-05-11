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

    // Array for boss spawn points
    public Transform[] spawnPoints;

    // Reference to boss model to move
    public GameObject bossChild;

    // Wait time in seconds for boss respawn after damage
    public float waitBeforeSpawn;

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
        } else
        {
           StartCoroutine(SpawnCoroutine() );
        }
        healthSlider.value = currentHealth;
    }

    // When boss takes damage, pause then respawn in a different position
    IEnumerator SpawnCoroutine()
    {
        bossChild.SetActive(false);
        yield return new WaitForSeconds(waitBeforeSpawn);
        int pointSelect = Random.Range(0, spawnPoints.Length);
        bossChild.transform.position = spawnPoints[pointSelect].position;
        bossChild.SetActive(true) ;
    }


}
