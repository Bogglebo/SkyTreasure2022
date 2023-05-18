using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Variable to hold the time the object should be alive
    public float lifetime;

    // Remove the game object after the specified time
    void Update()
    {
        Destroy(gameObject, lifetime);

    }
}
