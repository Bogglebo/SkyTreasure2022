using UnityEngine;

public class AttachPlatform : MonoBehaviour
{
    // Script to attach the player to the moving platform
    // (Requires Edit/Project Settings/Physics/Auto Sync Transforms
    // be be activated - (It is deactivated by default)
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        // Player becomes a child of the platform if it is moving
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player detatches from platform on exit
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
