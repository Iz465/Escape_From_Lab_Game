using System.Collections;
using UnityEngine;

public class BloodHealer : BloodEnemy
{
    private Collider[] corpseCount;
    [SerializeField]
    private LayerMask corpseLayer;
    protected override void Attack()
    {
        corpseCount = Physics.OverlapSphere(transform.position, 50f, corpseLayer);
        if (corpseCount.Length > 0)
        {
            Debug.Log("Resurrecting Enemies");
            Resurrect();
        }
        
        else
            Debug.Log("Nothing To Resurrect");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
     //   Gizmos.DrawWireSphere(transform.position, 50f);
    }

    private void Resurrect()
    {
        for (int i = 0; i < corpseCount.Length; i++)
        {
            Instantiate(Enemy.deadEnemies[i], corpseCount[i].transform.position, transform.rotation);
            Destroy(corpseCount[i].gameObject);
        }
        Enemy.deadEnemies.Clear();
    }

}
