using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class WarriorController : MonoBehaviour
{
    // How fast the warriors will move
    public float moveSpeed, turnSpeed;
    // Array of points the warriors will patrol around
    public Transform[] patrolPoints;
    // Patrol point the warrior is moving to
    public int currentPatrolPoint;
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

    // Get enemy health controller
    private WarriorHealthController warriorHealth;

    // Set up AI State system for warriors
    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isAttacking, isPausing
    };
    public AIState currentState;

    public float waitTime;  // How long the warrior should wait at a patrol point
    public float waitChance;  //  % chance AI waits or not
    private float waitCounter;  // Countdown how long before warrior stops waiting
    public float pauseTime; // Time to pause before switching AI state
    private float pauseCounter;  // Countdown for pauseTime
    public float hopForce, waitToChase;  // Time to pause before chasing player
    private float chaseWaitCounter; // Countdown for  wait to chase

    // Variables to set distance from player to trigger and end chase
    public float chaseDistance, pursuitSpeed, chaseRange;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.instance;  // Access the player controller static instance
        warriorHealth = WarriorHealthController.instance;  // Access the enemy health static instance
        currentState = AIState.isIdle;  // Set the warrior default state when the game starts
        waitCounter = waitTime;  // Initialise the wait Time for countdown
        chaseWaitCounter = waitToChase;
    }

    // Update is called once per frame
    void Update()
    {

        // Warrior action to take depending on AIState
        switch (currentState)
        {

            case AIState.isIdle:    // Warrior is idle

                // Set the warrior to stationary no vertical movement
                theRB.velocity = new Vector3(0f, theRB.velocity.y, 0f);
                // Countdown the wait time
                waitCounter -= Time.deltaTime;
                // If wait time has counted down, move to next patrol point
                if (waitCounter <= 0f)
                {
                    currentState = AIState.isPatrolling;
                    NextPatrolPoint();
                }
                break;  // Break out of switch statement and execute following line of code


            case AIState.isPatrolling:  // Warrior is patrolling

                yStore = theRB.velocity.y;
                // Calculate the target point - the current position
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                moveDirection.y = 0f; // No movement up and down
                moveDirection.Normalize(); // Normalize the move direction
                                           // Move the rigidbody on the warrior in the calculated direction
                                           // at the speed specified.  Unity will handle (fps) Time.deltaTime with the Rigidbody
                theRB.velocity = moveDirection * moveSpeed;
                theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);

                // Check proximity to destination point (within 1 world unit)
                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) <= 1f)
                {
                    NextPatrolPoint();
                }
                else  // Warrior is not at the next target
                {
                    lookTarget = patrolPoints[currentPatrolPoint].position;
                }

                break;


            case AIState.isChasing:  // Warrior is chasing the player

                lookTarget = player.transform.position;

                if (chaseWaitCounter > 0)
                {
                    chaseWaitCounter -= Time.deltaTime;
                }
                else
                {
                    yStore = theRB.velocity.y;
                    // Calculate the target point - the current position
                    moveDirection = player.transform.position - transform.position;
                    moveDirection.y = 0f; // No movement up and down
                    moveDirection.Normalize(); // Normalize the move direction
                                               // Move the rigidbody on the warrior in the calculated direction
                                               // at the speed specified.  Unity will handle (fps) Time.deltaTime with the Rigidbody
                    theRB.velocity = moveDirection * pursuitSpeed;
                    theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);
                }

                // Check if player is out of chase range
                if (Vector3.Distance(player.transform.position, transform.position) > chaseRange)
                {
                    currentState = AIState.isPausing;  // Pause before switching AI State
                    pauseCounter = pauseTime; //  Set time to pause
                    theRB.velocity = new Vector3();  // Stop the warrior from moving when paused
                }

                break;


            case AIState.isAttacking:   // Warrior is attacking the player

                break;


            case AIState.isPausing:    // Pause before switching AI state

                pauseCounter -= Time.deltaTime;

                if (pauseCounter < 0)
                {
                    currentState = AIState.isPatrolling;
                }
                break;


            default:
                break;
        }

        // Check relative distance to player to calculate whether to chase
        if (currentState != AIState.isChasing)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= chaseDistance)
            {
                currentState = AIState.isChasing;
                theRB.velocity = Vector3.up * hopForce;
                chaseWaitCounter = waitToChase;
            }
        }

        // Ignore the y axis (up and down)
        lookTarget.y = transform.position.y;

        // Look at the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
            (lookTarget - transform.position), turnSpeed * Time.deltaTime);

    }

    // Calculate the next patrol point
    public void NextPatrolPoint()
    {
        // Randomly decide whether to wait or not
        // If random number is less than waitChance set in insepctor, otherwise move to next point
        if (Random.Range(0f, 100f) < waitChance)
        {
            waitCounter = waitTime;
            currentState = AIState.isIdle;
        }
        else
        {
            currentPatrolPoint++;
            // Reset patrol point if it is greater than the size of the array
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
        }
    }

    // Check for collision between warrior and player
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {

    //        Debug.Log("Collision triggerred by Enemy " + collision.gameObject.name);


    //        HealthController.instance.Damage();
    //        AudioController.instance.PlayFX(7);
    //        PlayerController.instance.Bounce();
    //    }
    // }

    // Check for Player jumping on warrior head triggered by box collider  
    // attached to Player child's feet (Amy's)
    // Alternatively if the collider is the Player Parent, the warrior hit the player  first
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {  // Player damages warrior
            Debug.Log("OnTriggerEnger triggered by Player " + other.name);
            if (other.name == "Amy")
            {
                Debug.Log("This is where Amy hit the warrior on the head");
                warriorHealth.EnemyDamaged();

                

            } else
            {  // Warrior damages player
                if (other.name == "Player")
                {
                    Debug.Log("This is where the warrior damaged the player");
                    Debug.Log("Player still taking damage");
                    HealthController.instance.Damage();
                }
            }
            //Destroy(gameObject);
        }
    }



}
