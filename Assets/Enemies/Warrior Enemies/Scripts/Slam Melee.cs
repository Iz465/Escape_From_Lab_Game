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
    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canAttack) return;
        base.StartAttack(context);
        Attack();
        line.toggleCircle = true;

        canAttack = false;
        StartCoroutine(ResetSlam(cooldown));

    }

    public void HitGround()
    {

     
        line.toggleCircle = false;
        line.DisableCircle();

        CameraShake cameraShake = cam.GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.1f));

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
   //     Gizmos.DrawSphere(transform.position, 15);
    }



}
