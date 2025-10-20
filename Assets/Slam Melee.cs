using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlamMelee : BasePower
{
    private linescript line;
    private LayerMask enemyLayer;
    protected override void Start()
    {
        base.Start();
        line = GetComponent<linescript>();
    }
    public override void StartAttack(InputAction.CallbackContext context)
    {
      
        base.StartAttack(context);
        Attack();
        line.toggleCircle = true;
    }

    public void HitGround()
    {

        Debug.Log("HAMMER SLAMMED");
        line.toggleCircle = false;
        line.DisableCircle();

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, (5), enemyLayer);

        if (enemyColliders.Length == 0) return;
        HashSet<navmeshtestscript> enemiesHit = new HashSet<navmeshtestscript>();
        foreach (Collider collider in enemyColliders)
        {
            navmeshtestscript enemy = collider.GetComponentInParent<navmeshtestscript>();
            if (!enemy)
                enemy = collider.GetComponent<navmeshtestscript>();
            if (!enemy) return;
            enemiesHit.Add(enemy);
        }

        foreach (navmeshtestscript enemy in enemiesHit)
            enemy.TakeDamage(30);
        enemiesHit.Clear();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(transform.position, 5);
    }



}
