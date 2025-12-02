using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public float catchDistance = 1.5f; // How close before "catching" the player
    public float lookSpeed = 5f;      // How quickly it turns to face the player

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        if (distance <= catchDistance)
        {
            // "Catch" player
            Debug.Log("Player caught!");
            // respawn, game over, or damage effect here
        }
    }
}
