using UnityEngine;
using static BlockAttacks;

public class BlackKnightCastAttack : MonoBehaviour
{
    [SerializeField] private ParticleInUse particleInUse;


    // must use correct block to avoid damage
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player) 
        {
            Destroy(gameObject);
            return;
        }
   

        switch (particleInUse)
        {
            case ParticleInUse.red: if (BlockAttacks.particleInUse != ParticleInUse.red) player.TakeDamage(15); break;
            case ParticleInUse.green: if (BlockAttacks.particleInUse != ParticleInUse.green) player.TakeDamage(15); break;
            case ParticleInUse.blue: if (BlockAttacks.particleInUse != ParticleInUse.blue) player.TakeDamage(15); break;
        }

        Destroy(gameObject);
    }

}
