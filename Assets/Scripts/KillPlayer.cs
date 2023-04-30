using UnityEngine;


public class KillPlayer : MonoBehaviour
{
    // Check whether the Player has collided with the box collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Instance of the static SystemController 
            SystemController.instance.Respawn();
        }
    }
}
