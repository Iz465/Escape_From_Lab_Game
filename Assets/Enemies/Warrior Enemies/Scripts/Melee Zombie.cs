using System.Collections;
using UnityEngine;

public class MeleeZombie : navmeshtestscript
{

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip footstepSound;
  

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
            player.TakeDamage(2);
     
     
    }

 

    protected override void EnemyDeath()
    {
     
        base.EnemyDeath();
        globalEnemyManager.RespawnEnemyWave(GlobalEnemyManager.totalMeleeZombies, gameObject);
       
    }

    private void PlayMeleeScream()
    {
        int randomSound = Random.Range(0, attackSounds.Count);
        globalEnemyManager.CheckEnemySound(attackSounds[randomSound], "attack", audioSource); 
    }

    private void FootstepSound()
    {
        globalEnemyManager.CheckEnemySound(footstepSound, "footsteps", audioSource);
   
    }

    private void ExplodeSound()      
    {
        if (attackSound) audioSource.PlayOneShot(attackSound);
    }

    private void Explode()
    {

    }

}
