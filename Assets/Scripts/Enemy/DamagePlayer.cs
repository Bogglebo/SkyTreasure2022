using UnityEngine;

// Script to facilitate damage to player from items
public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioController.instance.PlayFX(3);
            HealthController.instance.Damage();
        }
        // Remove the box from the game if the player collides with it
        if (gameObject.CompareTag("DamageBox") &&
            !(other.gameObject.CompareTag("GoblinBlast")) &&
            !(other.gameObject.CompareTag("WarriorBoss")))
        {
            Destroy(gameObject);
        }
    }
}
