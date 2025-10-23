using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MeleeZombie : navmeshtestscript
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point1Second;
    [SerializeField] private Transform point2Second;
    [SerializeField] private Transform point1Third;
    [SerializeField] private Transform point2Third;
    [SerializeField] private LayerMask playerLayer;
    private bool checkHitbox;
    private bool canHit;


    protected override void Start()
    {
        base.Start();
  
        checkHitbox = false;
        canHit = false;
        rotateSpeed = 100;

    }



    protected override void AttackPlayer()
    {
        canAttack = false;
 

        animator.SetTrigger("MeleeCombo");


    }


    private void ResetAttack()
    {
     
        Debug.Log("Resetting");


        canAttack = true;
    }


    private void EnableHit()
    {
        canHit = true;
        checkHitbox = true;

        if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red)
            player.TakeDamage(5);

    }

    private void DisableHit()
    {
        canHit = false;
        checkHitbox = false;
        rotateSpeed = 5f;
    }


}
