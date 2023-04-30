using UnityEngine;

// Script to control enemy health
public class EnemyHealthController : MonoBehaviour
{

    // Create a static instance of the EnemyHealthController 
    // public static EnemyHealthController instance;

    // Int to count number of enemies killed up to 5
    public static int enemyCount = 0;

    // Maximum health of enemy
    public int enemyMaxHealth = 2;
    private int enemyCurrentHealth;
    // Enemy death effect
    public GameObject deathEffect;
    // Activate Level End portal and trigger
    public GameObject portal;
    public GameObject levelTrigger;

    private void Awake()
    {
        //instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        enemyCount = 0;
        portal.SetActive(false);
        levelTrigger.SetActive(false);
    }

    public void EnemyDamaged()
    {
        enemyCurrentHealth--;
        if (enemyCurrentHealth <= 0)
        {
            AudioController.instance.PlayFX(5);
            ScoreController.instance.UpdateScore(150);
            Destroy(gameObject);
            PlayerController.instance.Bounce();
            Instantiate(deathEffect, transform.position +
                new Vector3(0, 1.5f, 0f), transform.rotation);
            // When 5 enemies are killed open entry to the next level
            enemyCount++;
            if (enemyCount == 5)
            {
                portal.SetActive(true);
                levelTrigger.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyDamaged();
    }
}
