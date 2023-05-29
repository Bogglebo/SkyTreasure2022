using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiAgent : MonoBehaviour
{
    public Transform[] patrolPoints;    // Array of patrol points on the NavMesh 
    public float moveSpeed;     // Movement speed of enemy agent
    public float chaseRange;    // Distance before enemy will chase player
    private float distance;         // Actual Distance between enemy and player
    public Transform player;    // Transform of the player
    public Animator animator;   // Animator object

    private NavMeshAgent agent; // NavMesh agent component
    private PlayerController playerController;  // Access static instance of player
    private HealthController healthController;  // Access static  instance of player health

    public int currentPatrolPoint = 3;  // Patrol point the enemy is going to
    [SerializeField] private bool isPatrolling = true;  // Parameter to control patrol state
    [SerializeField] private bool isChasing = false;    // Parameter to control chase state
    [SerializeField] private bool isAttacking = false;  // Parameter to control attack state

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerController = PlayerController.instance;
        healthController = HealthController.instance;
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        NextPatrolPoint();
    }

    // Update is called once per frame
    private void Update()
    {
      // Check to see whether the player is within range
        distance = Vector3.Distance(transform.position, player.position);

        // Action to take for each state
        // Enemy is patrolling around the patrol points
        if (isPatrolling)
        {   // if nearing the patrol point, move to the next one
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                NextPatrolPoint();
            }
        }
        else if (isChasing)     // The player is within the chase range set in the inspector
        {
            agent.SetDestination(player.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isAttacking = true;
            }
        } else if (isAttacking )    // The player is within attack range
        {
            isChasing = false;
            isAttacking = true;
        }

        //// Check to see whether the player is within range
        //distance = Vector3.Distance(transform.position, player.position);

        // If the player is within chase range, set up the chase
        if (distance <= chaseRange) 
        {
            agent.isStopped = false;
            isChasing = true;
            isAttacking = false;
            isPatrolling = false;
            animator.SetBool("isRunning", true);

            // If the enemy is within attacking range
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                animator.SetBool("isRunning", false) ;
                animator.SetBool("isAttacking", true) ;
                isChasing = false;
                isPatrolling = false;
                isAttacking = true;
            }
        } else
        // The enemy is patrolling
        {
            agent.isStopped = false;
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
            isChasing = false;
            isAttacking = false;
            isPatrolling = true;
        }
    }

    private void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        agent.isStopped = false;
        animator.SetBool("isRunning", true);
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    //private void Attack()
    //{
    //    if (distance > agent.stoppingDistance && distance <= chaseRange)
    //    {
    //        isPatrolling = false;
    //        agent.isStopped = false;
    //        isChasing = true;
    //        isAttacking = false;
    //        animator.SetBool("isAttacking", false);
    //        animator.SetBool("isRunning", true );
    //     } else if (distance > agent.stoppingDistance && distance >= chaseRange)
    //    {
    //        isPatrolling = true;
    //        agent.isStopped = false;
    //        isChasing = false;
    //        isAttacking = false;
    //        animator.SetBool("isAttacking", false);
    //        animator.SetBool("isRunning", true);
    //     } else
    //    {
    //        Debug.Log("Within attack range so attack the player");
    //        isPatrolling = false;
    //        agent.isStopped = true;
    //        isChasing = false;
    //        isAttacking = true;
    //        animator.SetBool("isAttacking", true);
    //        animator.SetBool("isRunning", false);
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPatrolling = false;
            isChasing = true;
            agent.isStopped = true;
            Debug.Log("In onTriggerEnter");

            //animator.SetBool("isPatrolling", false);
            //animator.SetBool("isRunning", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPatrolling = true;
            isChasing = false;
            isAttacking = false;
            agent.isStopped = false;

            //animator.SetBool("isPatrolling", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }
}
