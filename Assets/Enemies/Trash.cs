using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Trash : MonoBehaviour
{
    public Transform enemy;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        StartCoroutine(Walk());
    }

    IEnumerator Walk()
    {
        while (true) 
        { 
            agent.SetDestination(enemy.position);
            yield return new WaitForSeconds(5);
        }

    }
}