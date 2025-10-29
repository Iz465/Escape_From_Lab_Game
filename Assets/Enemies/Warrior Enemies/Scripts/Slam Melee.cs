using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlamMelee : BasePower
{
    private linescript line;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float cooldown;
    bool canAttack = true; 
    protected override void Start()
    {
        base.Start();
        line = GetComponent<linescript>();
    }

    // slam ability can only be called once its been reset.
    // line renderer shows a visual of the hammer attack radius
    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canAttack) return;
        base.StartAttack(context);
        line.toggleCircle = true;

        canAttack = false;
        StartCoroutine(ResetSlam(cooldown));

    }

    // Damages any enemies that are in the hammer radius the moment it hits the ground. 
    public void HitGround()
    {

     
        line.toggleCircle = false;
        line.DisableCircle();

        CameraShake cameraShake = cam.GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.5f, 0.5f, 0.1f));

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, (15), enemyLayer);

        if (enemyColliders.Length == 0)
        {
            Debug.Log("No Enemies in hammer radius");
            return;
        }
      
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
            enemy.TakeDamage(stats.damage);
        enemiesHit.Clear();
        
    }

    private IEnumerator ResetSlam(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("HAMMER RESET");
        canAttack = true;
    }


    // shows in the editor how big the radius will be
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
   //     Gizmos.DrawSphere(transform.position, 15);
    }



}
