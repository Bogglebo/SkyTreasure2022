using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorController : MonoBehaviour
{
    // How fast the warriors will move
    public float moveSpeed, turnSpeed;
    // Array of points the warriors will patrol around
    public Transform[] patrolPoints;
    // Patrol point the warrior is moving to
    public int currentPatrolPoint ;
    // Direction we want the warrior to move towards
    private Vector3 moveDirection;
    // Variable to stop enemy looking up and down
    private Vector3 lookTarget;
    // Store a value to smooth rigidbody y value 
    public float yStore;
    // Nav Mesh Agent
    NavMeshAgent agent;

    // Rigidbody to move the warriors around with basic physics
    public Rigidbody theRB;

    // Get player controller
    private PlayerController player;

    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        yStore = theRB.velocity.y;
        // Calculate the target point - the current position
        moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
        moveDirection.y = 0f; // No movement up and down
        moveDirection.Normalize(); // Normalize the move direction
        // Move the rigidbody on the warrior in the calculated direction
        // at the speed specified.  Unity will handle (fps) Time.deltaTime with the Rigidbody
        theRB.velocity = moveDirection * moveSpeed;
        theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);

        // Check proximity to destination point (within .1 world unit)
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) <= 1f)
        {
            NextPatrolPoint();
        }
        else  // Warrior is not at the next target
        {
            lookTarget = patrolPoints[currentPatrolPoint].position;
        }
        // lookTarget = player.transform.position;

        // Ignore the y axis (up and down)
        lookTarget.y = transform.position.y;


        // Look at the target
        //transform.LookAt(lookTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
            (lookTarget - transform.position), turnSpeed * Time.deltaTime);

    }

    // Calculate the next patrol point
    public void NextPatrolPoint()
    {
        currentPatrolPoint++;
        // Reset patrol point if it is greater than the size of the array
        if (currentPatrolPoint >= patrolPoints.Length)
        {
            currentPatrolPoint = 0;
        }
    }
}
