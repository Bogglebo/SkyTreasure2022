using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    // Reset the player spawn position when she reaches a checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SystemController.instance.SetSpawnPoint(transform.position);
        }
    }
}
