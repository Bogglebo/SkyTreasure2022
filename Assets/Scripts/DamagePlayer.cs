using UnityEngine;

// Script to facilitate damage to player
public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioController.instance.PlayFX(3);
            HealthController.instance.Damage();
        }
        if (gameObject.CompareTag("DamageBox")  && 
            !(other.gameObject.CompareTag("GoblinBlast")))
        {
            Destroy(gameObject);
        }
    }
}
