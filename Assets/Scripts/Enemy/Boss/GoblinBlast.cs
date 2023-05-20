using UnityEngine;

public class GoblinBlast : MonoBehaviour
{
    // Variables to fire the spell
    public float moveSpeed;
    public Rigidbody theRB;


    // Start is called before the first frame update
    void Start()
    {
        // Aim the spell towards the middle of the player
        transform.LookAt(PlayerController.instance.transform.position + Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        // Fire the spell in a forward direction
        theRB.velocity = transform.forward * moveSpeed;
        // Delete the spell after 10 seconds whether or not it collides (to prevent lost instances
        // sticking around in the scene forever
        Destroy(gameObject, 10);
    }

    // Action to take if spell hits player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthController.instance.Damage();
        }
        Destroy(gameObject);
    }
}
