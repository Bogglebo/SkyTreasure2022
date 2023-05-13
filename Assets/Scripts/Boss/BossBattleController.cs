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

    // Variable to ensure boss respawns at a different point
    private int lastSpawnPoint;

    // Object to activate particle effect on the boss
    public GameObject bossEffect;

    // Object to activate exit portal to next level
    public GameObject activateExitPortal;

    // Spell object for casting
    public GameObject theSpell;
    public Transform spellPoint;
    public float timeBetweenSpells;
    private float spellCounter;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Activate particle effect when boss appears
        if (bossEffect != null)
        {
            Instantiate(bossEffect, bossChild.transform.position, bossChild.transform.rotation);
        }

        spellCounter = timeBetweenSpells;

    }

    // Update is called once per frame
    void Update()
    {
        if (bossChild.activeSelf)
        {
            bossChild.transform.LookAt(PlayerController.instance.transform);
            // Amend Boss rotation so he doesn't lay down when Player is in the air
            bossChild.transform.rotation = Quaternion.Euler(0f, bossChild.transform.rotation.eulerAngles.y,
                0f);
            spellCounter -= Time.deltaTime;
            if (spellCounter <= 0)
            {
                Instantiate (theSpell, spellPoint.position, spellPoint.rotation);
                spellCounter = timeBetweenSpells;
            }
        }
    }

    public void DamageBoss()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            AudioController.instance.PlayFX(2);
            gameObject.SetActive(false);
            currentHealth = 0;
            activateExitPortal.SetActive(true);
        } else
        {
           StartCoroutine(SpawnCoroutine() );
        }
        AudioController.instance.PlayFX(7);
        healthSlider.value = currentHealth;
    }

    // When boss takes damage, pause then respawn in a different position
    IEnumerator SpawnCoroutine()
    {
        bossChild.SetActive(false);
        // Activate the particle effect on the boss when he changes position
        if (bossEffect != null)
        {
            Instantiate(bossEffect, bossChild.transform.position, bossChild.transform.rotation);
        }
        yield return new WaitForSeconds(waitBeforeSpawn);

        int pointSelect = Random.Range(0, spawnPoints.Length);

        // Variable used to check the boss respawn while loop doesn't repeat indefinitely
        int checkPosition = 0;

        while (pointSelect == lastSpawnPoint &&  checkPosition < 100)
        {
            pointSelect = Random.Range(0, spawnPoints.Length);
            checkPosition++;
        }

        lastSpawnPoint = pointSelect;

        bossChild.transform.position = spawnPoints[pointSelect].position;
        bossChild.SetActive(true) ;

        // When boss reappears activate boss particle effect
        if (bossEffect != null)
        {
            Instantiate(bossEffect, bossChild.transform.position, bossChild.transform.rotation);
        }

    }


}
