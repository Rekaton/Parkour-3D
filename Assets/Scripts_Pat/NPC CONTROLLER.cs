using UnityEngine;
using UnityEngine.AI;
public class NPCCONTROLLER : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {
        agent.SetDestination(target.position);
    }
}
