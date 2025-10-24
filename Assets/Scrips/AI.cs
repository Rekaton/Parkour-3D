using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public float chaseRange = 10f;    // How far the AI can "see" the player
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

        if (distance <= chaseRange)
        {
            // Follow player
            agent.SetDestination(player.position);

            // Rotate smoothly toward player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
        }

        if (distance <= catchDistance)
        {
            // "Catch" player
            Debug.Log("Player caught!");
            // You can trigger a respawn, game over, or damage effect here
        }
    }
}

