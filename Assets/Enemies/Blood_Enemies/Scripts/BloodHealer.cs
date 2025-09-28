using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BloodHealer : BloodEnemy
{
    private Collider[] corpseCount;
    [SerializeField] private LayerMask corpseLayer;
    private HashSet<GameObject> uniqueCorpse = new HashSet<GameObject>();
    protected override void Attack()
    {
        corpseCount = Physics.OverlapSphere(transform.position, 50f, corpseLayer);

        foreach (Collider collider in corpseCount)
        {
            GameObject corpse = collider.transform.root.gameObject;
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

    private void Resurrect()
    {
        foreach (GameObject corpse in uniqueCorpse)
        {
            if (navmeshtestscript.deadEnemies[0])
                Instantiate(navmeshtestscript.deadEnemies[0], corpse.transform.position, transform.rotation);
            Destroy(corpse);
        }
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
        canAttack = true;
      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 50f);
    }

}
