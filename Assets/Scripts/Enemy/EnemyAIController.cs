using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyAIController : MonoBehaviour
{

    private NavMeshAgent agent;                 //  Nav mesh agent component
    public Animator animator;
    public float startWaitTime = 2f;               //  Pause time for each action
    public float chaseWaitTime = 2f;             //  Wait time to chase player
    public float walkSpeed = 4f;                     //  Walk speed,
    public float runSpeed = 6f;                       //  Run speed


    public Transform[] patrolPoints;             //  Array of patrol points the enemy AI will walk around on the navmesh
    [SerializeField]
    int currentPatrolPoint;                             //  The next targetted patrol point 

    Vector3 playerLastPosition = Vector3.zero;   //  Player's last position near enemy
    Vector3 playerPosition;                       //  Last known position of the player

    float waitCounter;                                //  Wait time counter
    float waitToChase;                               //  Wait time before chasing the player
    [SerializeField]bool playerInRange;                            //  Flag when the player is in chase range
    [SerializeField] bool playerNearby;                              //  Flag when the player and enemy are nearby
    [SerializeField] bool isPatrolling;                                  //  Flag when the enemy is patrolling
    [SerializeField] bool isCaught;                                      //  Flag when the player is caught


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();  // Get the animator component
        playerPosition = Vector3.zero;      // Set the player transform to 0
        isPatrolling = true;                            // Flag the enemy as patrolling
        // Set the inital bool states to false
        isCaught = false;                             
        playerInRange = false;
        playerNearby = false;
        waitCounter = startWaitTime;             // Set the initial wait time
        waitToChase = chaseWaitTime;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.speed = walkSpeed;    //  Align the navmesh agent speed with the enemy speed
       // Set the first patrol point as the destination
        agent.SetDestination(patrolPoints[currentPatrolPoint].position); 
    }

    // Update is called once per frame
    private void Update()
    {
        // Ascertain the position of the enemy
        //EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision
        // If the enemy is not patrolling
        if (!isPatrolling)
        {
            Chasing();
        }
        else
        {
            animator.SetBool("IsMoving", true);
            Patroling();
        }
    }

    private void Chasing()
    {
        //  The enemy is chasing the player
        animator.SetBool("IsMoving", true);
        playerNearby = false;                       //  Set false that the player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!isCaught)
        {
            animator.SetBool("IsMoving", true);
            Move(runSpeed);
            agent.SetDestination(playerPosition);          //  set the destination of the enemy to the player location
        }
        if (agent.remainingDistance <= agent.stoppingDistance)    //  Control if the enemy arrive to the player location
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("Attack");
        {
            if (waitCounter <= 0 && !isCaught && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                isPatrolling = true;
                playerNearby = false;
                Move(walkSpeed);
                waitToChase = chaseWaitTime;
                waitCounter = startWaitTime;
                animator.SetBool("IsMoving", true);
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    animator.SetBool("IsMoving", false);
                    Stop();
                waitCounter -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (playerNearby)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (waitToChase <= 0)
            {
                animator.SetBool("IsMoving", true);
                Move(walkSpeed);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                animator.SetBool("IsMoving", false);
                Stop();
                waitToChase -= Time.deltaTime;
            }
        }
        else
        {
            playerNearby = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);    //  Set the enemy destination to the next waypoint
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                animator.SetBool("IsMoving", false );
                if (waitCounter <= 0)
                {
                    animator.SetBool("IsMoving", true);
                    NextPoint();
                    Move(walkSpeed);
                    waitCounter = startWaitTime;
                }
                else
                {
                    Stop();
                    waitCounter -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {

    }

    public void NextPoint()
    {
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
    }

    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }

    void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }

    void CaughtPlayer()
    {
        isCaught = true;
    }

    void LookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (waitCounter <= 0)
            {
                playerNearby = false;
                Move(walkSpeed);
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                waitCounter = startWaitTime;
                waitToChase = chaseWaitTime;
            }
            else
            {
                Stop();
                waitCounter -= Time.deltaTime;
            }
        }
    }

    //void EnviromentView()
    //{
    //    Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

    //    for (int i = 0; i < playerInRange.Length; i++)
    //    {
    //        Transform player = playerInRange[i].transform;
    //        Vector3 dirToPlayer = (player.position - transform.position).normalized;
    //        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
    //        {
    //            float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
    //            if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
    //            {
    //                this.playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
    //                isPatrolling = false;                 //  Change the state to chasing the player
    //            }
    //            else
    //            {
    //                /*
    //                 *  If the player is behind a obstacle the player position will not be registered
    //                 * */
    //                this.playerInRange = false;
    //            }
    //        }
    //        if (Vector3.Distance(transform.position, player.position) > viewRadius)
    //        {
    //            /*
    //             *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
    //             *  Or the enemy is a safe zone, the enemy will no chase
    //             * */
    //            this.playerInRange = false;                //  Change the sate of chasing
    //        }
    //        if (this.playerInRange)
    //        {
    //            /*
    //             *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
    //             * */
    //            playerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
    //        }
//}
    //}
}
