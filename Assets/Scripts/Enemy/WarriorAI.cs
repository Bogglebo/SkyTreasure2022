using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class WarriorAI : MonoBehaviour
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
    
    //// Store a value to smooth rigidbody y value 
    //public float yStore;

    // Nav Mesh Agent
    NavMeshAgent agent;

    // Store the agent's original position
    private Vector3 originalPosition;

    public Animator animator;

    // Rigidbody to move the warriors around with basic physics
    //public Rigidbody theRB;

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
    public float waitToChase;  // Time to pause before chasing player
    private float chaseWaitCounter; // Countdown for  wait to chase

    // Variables to set distance from player to trigger and end chase
    public float chaseDistance, pursuitSpeed, chaseRange;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.instance;  // Access the player controller static instance
        warriorHealth = WarriorHealthController.instance;  // Access the enemy health static instance
        animator = GetComponent<Animator>();
        currentState = AIState.isPatrolling;  // Set the warrior default state when the game starts
        waitCounter = waitTime;  // Initialise the wait Time for countdown
        chaseWaitCounter = waitToChase;
        originalPosition = transform.position; // Warrior's original position
    }

    // Update is called once per frame
    void Update()
    {
        
        // Warrior action to take depending on AIState
        switch (currentState)
        {

            case AIState.isIdle:    // Warrior is idle
               animator.SetBool("IsMoving", false);
               // Check to see if the warrior has reached its destination 
               // if it has, set the destination to the current position and stop movement
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    animator.SetBool("IsMoving", false);
                    agent.destination = transform.position;
                    agent.autoBraking = false;      // Disable auto braking
                } else  // If the warrior hasn't reached its target enable auto-braking to facilitate
                // a smooth stop at the destination
                {
                    animator.SetBool("IsMoving", true);
                    agent.autoBraking = true;
                }
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
                // Calculate the target point - the current position
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                animator.SetBool("IsMoving", true);
                // Move the warrior to the patrol point
                agent.SetDestination(moveDirection);

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
                
                // Invoke Run animation
                animator.SetBool("IsMoving", true);

                lookTarget = player.transform.position;

                if (chaseWaitCounter > 0)
                {
                    chaseWaitCounter -= Time.deltaTime;
                }
                else
                {
                    // Calculate the target point - the current position
                    moveDirection = player.transform.position - transform.position;
                    agent.SetDestination(moveDirection);
                }

                // Check if player is out of chase range
                if (Vector3.Distance(player.transform.position, transform.position) > chaseRange)
                {
                    agent.SetDestination(moveDirection);
                }

                break;


            case AIState.isAttacking:   // Warrior is attacking the player
                // Stop the run animation and play the attack one
                animator.SetBool("IsMoving", false);
                animator.SetTrigger("Attack");
                Debug.Log("Warrior is Attacking " + player.name);
                               //HealthController.instance.Damage();

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
            currentPatrolPoint++;
            // Reset patrol point if it is greater than the size of the array
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
    }


    // Check for Player jumping on warrior head triggered by box collider  
    // attached to Player child's feet (Amy's)
    // Alternatively if the collider is the Player Parent, the warrior hit the player  first
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //animator.SetBool("IsMoving", false);
            //animator.SetTrigger("Attack");
            // Player damages warrior
            Debug.Log("OnTriggerEnter triggered with Player " + other.name);

            if (other.name == "Amy")  // The box collider on the player childs feet
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
                    currentState = AIState.isAttacking;
                   
                }
            }
            //Destroy(gameObject);
        }
    }
}
