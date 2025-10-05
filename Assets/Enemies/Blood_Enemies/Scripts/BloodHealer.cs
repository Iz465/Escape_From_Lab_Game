using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodHealer : BloodEnemy
{
    private Collider[] corpseCount;
    [SerializeField] private LayerMask corpseLayer;
    private HashSet<GameObject> uniqueCorpse = new HashSet<GameObject>();
    private bool canRevive;
    protected override void Attack()
    {
        corpseCount = Physics.OverlapSphere(transform.position, 50f, corpseLayer);

        foreach (Collider collider in corpseCount)
        {
            GameObject corpse = collider.transform.root.gameObject;
          //  GameObject corpse = collider.gameObject;
            uniqueCorpse.Add(corpse);
        }

        if (uniqueCorpse.Count > 0)
        {
            Debug.Log("Resurrecting Enemies");
            Resurrect();
        }
        
        else
            Debug.Log("Nothing To Resurrect");
    }

    protected override void Start()
    {
        base.Start();
        canRevive = true;
    }

    protected override void ChasePlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0; // keeps horizontal rotation only
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotateSpeed);


        if (distanceToPlayer > attackRange && canRevive)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        
        }

        else if (distanceToPlayer <= attackRange)
        {

            if (canRevive)
            {
                agent.isStopped = true;
                AttackPlayer();
            }
        }

    }

    protected override void AttackPlayer()
    {
        animator.SetBool("CanAttack", true);
        canRevive = false;
    }

    private void Resurrect()
    {
  
        foreach (GameObject corpse in uniqueCorpse)
        {
            if (navmeshtestscript.deadEnemies[0])
                Debug.Log($"Corpse name is : {corpse}");
                Instantiate(navmeshtestscript.deadEnemies[0], corpse.transform.position, transform.rotation);
            navmeshtestscript.deadEnemies.RemoveAt(0);
            Destroy(corpse);
        } 
        uniqueCorpse.Clear();
        navmeshtestscript.deadEnemies.Clear();
      
    }


    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);
        StartCoroutine(ResetAttack(20f));

    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canRevive = true;
      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 50f);
    }

}
