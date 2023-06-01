using UnityEngine;
using UnityEngine.AI;


public class WarriorBossController : MonoBehaviour
{
    public static int enemyCount;

    // Enemy death effect
    public GameObject deathEffect;
    // Activate Level End portal and trigger
    public GameObject portal;
    public GameObject levelTrigger;

    public Transform[] patrolPoints;    // Array of patrol points on the NavMesh 
    public float moveSpeed;     // Movement speed of enemy agent
    public float chaseRange;    // Distance before enemy will chase player
    private float distance;         // Actual Distance between enemy and player
    public Transform player;    // Transform of the player
    public Animator animator;   // Animator object

    private NavMeshAgent agent; // NavMesh agent component
    private PlayerController playerController;  // Access static instance of player
    private HealthController healthController;  // Access static  instance of player health
    private WarriorHealthController warriorHealth;

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
        warriorHealth = WarriorHealthController.instance;
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
        }
        else if (isAttacking)    // The player is within attack range
        {
            isChasing = false;
            isAttacking = true;
        }

        // If the player is within chase range, set up the chase or attack
        // depending on distance
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
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", true);
                isChasing = false;
                isPatrolling = false;
                isAttacking = true;
            }
        }
        else
        // If the enemy isn't chasing or attacking it is patrolling
        {
            agent.isStopped = false;
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
            isChasing = false;
            isAttacking = false;
            isPatrolling = true;
        }
    }

    // Move to the next patrol point
    private void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        agent.isStopped = false;
        animator.SetBool("isRunning", true);
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        // Increase the current patrol point, use the modulo operation  to ensure
        // the next patrol point becomes 0 if max patrol points array length is reached
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    public void EnemyDamaged()
    {
        AudioController.instance.PlayFX(5);
        ScoreController.instance.UpdateScore(150);
        Destroy(gameObject);
        PlayerController.instance.Bounce();
        Instantiate(deathEffect, transform.position +
            new Vector3(0, 1.5f, 0f), transform.rotation);
        // When 5 enemies are killed open entry to the next level
        enemyCount++;
        if (enemyCount == 5)
        {
            portal.SetActive(true);
            levelTrigger.SetActive(true);
        }
    }

    // Check for Player jumping on warrior head triggered by box collider  
    // attached to Player child's feet (Amy's)
    // Damage the Player via a box collider on the melee weapon
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal melee damage to the enemy
            if (other.name == "Amy")  // The box collider on the player childs feet
            {
                EnemyDamaged();
            }
            else
            {  // Deal melee damage to the player from the box collider on the sword
                if (other.name == "Player")
                {
                    HealthController.instance.Damage();
                }
            }
        }
    }
}
