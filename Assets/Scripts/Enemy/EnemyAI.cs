using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Script to control the Enemy Movement

public class EnemyAI : MonoBehaviour
{

    // Position of the object to pursue (player) 
    [SerializeField] Transform target;

    // NavMesh Agent (enemy)
    NavMeshAgent agent;

    // Proximity of the player to the pursuit target in unity units
    [SerializeField] float chaseRange;

    // Variable to hold calculated distance from target initialised to a high number
    // otherwise it would trigger on start as it would be zero
    float distanceToTarget = Mathf.Infinity;

    // Get animator component
    Animator animator;

    public bool isProvoked = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the target and the enemy each frame
        distanceToTarget = Vector3.Distance(target.position, agent.transform.position);

        // If the player is within the chase range set, pursue the player
        if (distanceToTarget < chaseRange)
        {
            isProvoked = true;
            EngageTarget();
        }
    }

    // Method to decide whether the enemy should chase or attack the player
    private void EngageTarget()
    {
        if (distanceToTarget >= agent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= agent.stoppingDistance)
            {
                AttackTarget();
        }
    }

    // Method to call when enemy is chasing the player
    private void ChaseTarget()
    {
        animator.SetTrigger("run");
        agent.SetDestination(target.position);  // Move the enemy to the target's position
    }

    // Method to call if enemy gets within attack range of the player
    private void AttackTarget()
    {
        animator.SetBool("Attack", true);
        Debug.Log("Enemy " + name + (" is attacking the  " + target.name ));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

}
