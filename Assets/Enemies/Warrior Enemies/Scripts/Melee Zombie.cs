using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MeleeZombie : navmeshtestscript
{

    [SerializeField] private LayerMask playerLayer;
 

    protected override void Start()
    {
        base.Start();
        GlobalEnemyManager.totalMeleeZombies.Add(gameObject);
    }


    private void ResetAttack()
    {
        canAttack = true;
    }


    // Window during attack animation that enemy can damage
    private void EnableHit()
    {
        if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red)
            player.TakeDamage(5);
    }

 

    protected override void EnemyDeath()
    {
     
        base.EnemyDeath();
        globalEnemyManager.EmptyMeleeZombies(gameObject);
       
    }

}
