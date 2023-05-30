using UnityEngine;

public class BossActivator : MonoBehaviour
{
    // Action to take when the player enters the boss area
    public GameObject actionBoss;

    private void OnTriggerEnter(Collider other)
    {
        // When player enters the area activate the boss
        if (other.CompareTag("Player"))
        {
            // Make the boss appear
            AudioController.instance.PlayFX(7);
            actionBoss.SetActive(true);

            // Deactivate the trigger area once the boss is active
            gameObject.SetActive(false);
        }
    }
}
