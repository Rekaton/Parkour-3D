using UnityEngine;
using UnityEngine.AI;
public class NPCCONTROLLER : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public float range = 25f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {

        if (target == null)
        {
            return;
        }
        agent.SetDestination(target.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position-transform.position, out hit, range))
        {
            if (hit.transform == target)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}
