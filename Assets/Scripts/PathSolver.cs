using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PathSolver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 desination = GameObject.Find("FinishDoor").transform.position;
        NavMeshAgent agent = transform.GetComponent<NavMeshAgent>();

        agent.SetDestination(desination);
        bool hasPath = agent.hasPath;

        print("has path: "+hasPath.ToString());
        print("path");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
