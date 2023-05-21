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


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the target and the enemy each frame
        distanceToTarget = Vector3.Distance (target.position, agent.transform.position);
        // If the player is within the chase range set, pursue the player
        if (distanceToTarget < chaseRange)
        {
            agent.SetDestination(target.position); // Move the enemy to the target's position
        }
    }
}
