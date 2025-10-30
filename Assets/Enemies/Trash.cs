using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Trash : MonoBehaviour
{
    public Transform enemy;
    public float health;
    float lastDamage;

    private void Start()
    {
        //StartCoroutine(Chase());
    }

    IEnumerator Chase()
    {
        while (true)
        {
            GetComponent<NavMeshAgent>().SetDestination(enemy.position);
            yield return new WaitForSeconds(1);

        }
    }

    public bool TakeDamage()
    {
        if(Time.time > lastDamage)
        {
            lastDamage = Time.time + 1;
            health -= 5;
            return true;
        }
        return false;
    }
}