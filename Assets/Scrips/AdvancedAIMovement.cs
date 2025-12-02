using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdvancedAIMovement : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public List<Transform> patrolPoints;

    [Header("AI Settings")]
    public float sightRange = 15f;
    public float sightAngle = 60f;
    public float chargeRange = 10f;
    public float patrolWaitTime = 2f;
    public LayerMask obstacleMask;

    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private Vector3 lastKnownPlayerPos;
    private bool playerSeen = false;
    private bool searchingLastPos = false;

    private enum AIState 
    { 
        Patrolling, 
        Chasing, 
        Searching 
    }
    private AIState currentState = AIState.Patrolling;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Count > 0)
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrolling:
                Patrol();
                break;
            case AIState.Chasing:
                Chase();
                break;
            case AIState.Searching:
                Search();
                break;
        }

        DetectPlayer();
    }


    void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                waitTimer = 0f;
            }
        }
    }

    void Chase()
    {
        if (playerSeen)
        {
            agent.SetDestination(player.position);
            lastKnownPlayerPos = player.position;
        }
        else
        {
            currentState = AIState.Searching;
            searchingLastPos = true;
            agent.SetDestination(lastKnownPlayerPos);
        }
    }

    void Search()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            searchingLastPos = false;
            currentState = AIState.Patrolling;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    
    void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= sightRange && Vector3.Angle(transform.forward, directionToPlayer) < sightAngle / 2)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                playerSeen = true;
                lastKnownPlayerPos = player.position;

                if (distanceToPlayer <= chargeRange)
                {
                    currentState = AIState.Chasing;
                }
                return;
            }
        }
        
        if (playerSeen && distanceToPlayer > sightRange)
        {
            playerSeen = false;
        }

        if (!playerSeen && currentState == AIState.Chasing)
        {
            currentState = AIState.Searching;
            agent.SetDestination(lastKnownPlayerPos);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chargeRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, sightAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * sightRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * sightRange);
    }
}
