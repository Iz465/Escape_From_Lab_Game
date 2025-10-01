using UnityEngine;
using UnityEngine.AI;

public class ChaseAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected void Chase(Transform obj)
    {
        agent.SetDestination(obj.position);
    }

    protected bool DetectObj(Transform obj)
    {
        //only return true if obj is found
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (obj.position - transform.position), out hit))
        {
            if (hit.transform == obj)
                return true;
            return false;
        }
        return false;
    }

    protected bool IsInFov(Transform obj)
    {
        Vector3 direction = obj.position - transform.position;

        float fov = Vector3.Dot(transform.forward, direction.normalized);
        
        return fov > 0.5f;
    }
}
