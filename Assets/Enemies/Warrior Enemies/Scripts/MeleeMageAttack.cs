using UnityEngine;

public class MeleeMageAttack : PlayerHitDetection
{
    // Must block with blue to not take damage from this attack
    protected override void OnCollisionEnter(Collision collision)
    {
        if (BlockAttacks.particleInUse == BlockAttacks.ParticleInUse.blue) 
            Destroy(gameObject);

        else
            base.OnCollisionEnter(collision);
    }
}
