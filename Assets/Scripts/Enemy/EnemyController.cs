using UnityEngine;
using UnityEngine.AI;

// Script to control enemies
public class EnemyController : MonoBehaviour
{
    // List of points the enemy can go to
    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    // Reference to the enemy agent
    public NavMeshAgent agent;
    // Reference to the animator
    public Animator animator;
    // Enumerator to check the enemy state
    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isAttacking
    };
    public AIState currentState;   // Current AI State
    public float waitAtPoint = 2f;  // Wait at patrol point for 2 seconds
    private float waitCounter;
    public float chaseDistance;    // Distance between player & enemy
    public float attackRange = 1f;     // Distance from player to trigger attack
    public float attackDelay = 2f;      // Time between enemy attacks
    public float attackCounter;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
        currentPatrolPoint = Random.Range(0, patrolPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        // How far away from the player is the enemy
        float distanceToPlayer = Vector3.Distance(transform.position,
            PlayerController.instance.transform.position);

        switch (currentState)
        {
            case AIState.isIdle:    // Wait at each patrol point
                animator.SetBool("IsMoving", false);
                if (waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    currentState = AIState.isPatrolling;
                    // Set the patrol destination
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = AIState.isChasing;
                    animator.SetBool("IsMoving", true);
                }

                break;

            case AIState.isPatrolling:
                // Check to see how far away we are
                if (agent.remainingDistance <= .2f)
                {
                    // Go to the next patrol point
                    currentPatrolPoint++;
                    // Check it's not the final patrol point
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }
                    // agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;
                }
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = AIState.isChasing;
                }
                // Set the moving animation
                animator.SetBool("IsMoving", true);
                break;
            case AIState.isChasing:
                agent.SetDestination(PlayerController.instance.transform.position);
                // Trigger attack
                if (distanceToPlayer <= attackRange)
                {
                    currentState = AIState.isAttacking;
                    animator.SetTrigger("Attack");
                    animator.SetBool("IsMoving", false);
                    // Stop the enemy moving
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    attackCounter = attackDelay;
                }
                // Stop chasing player if they are outside range
                if (distanceToPlayer > chaseDistance)
                {
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;
                    agent.velocity = Vector3.zero;
                    agent.SetDestination(transform.position);
                }
                break;
            case AIState.isAttacking:
                // Ensure the enemy faces the player
                transform.LookAt(PlayerController.instance.transform, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                // If the enemy is still near the player attack again
                attackCounter -= Time.deltaTime;
                if (attackCounter <= 0)
                {
                    if (distanceToPlayer < attackRange)
                    {
                        animator.SetTrigger("Attack");
                        attackCounter = attackDelay;
                    }
                    else  // Switch back to idle patrolling 
                    {
                        currentState = AIState.isIdle;
                        waitCounter = waitAtPoint;
                        agent.isStopped = false;
                    }
                }
                break;
        }
    }


}

