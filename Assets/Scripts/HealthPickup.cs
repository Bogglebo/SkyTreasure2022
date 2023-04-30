using UnityEngine;

// Script to control health powerup
public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public bool fullHeal;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (fullHeal)       // Heal to full amount
            {
                HealthController.instance.ResetHealth();
            }
            else                 // Heal by a specified amount
            {
                HealthController.instance.AddHealth(healAmount);
            }
        }
    }


}
