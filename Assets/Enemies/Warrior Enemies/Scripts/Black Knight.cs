using UnityEngine;
using UnityEngine.Rendering;

public class BlackKnight : navmeshtestscript
{
    [SerializeField] private ParticleSystem swordParticle;
    [SerializeField] private Transform swordLocation;
    private int[] storedNumber = new int[1];
    int oldNumber;
    bool doOnce = true;
  


    // The same attack never repeats
    private void SwitchAttackType()
    {
        
        oldNumber = storedNumber[0];

        while (oldNumber == storedNumber[0])
        {
            int randomNumber = Random.Range(0, 3);
            storedNumber[0] = randomNumber;
        }

        // Added this as it doesnt work on the first time.
        if (doOnce)
        {
            SelectColour();

            oldNumber = storedNumber[0];
            doOnce = false;
        }

        SelectColour();




    }

    // Particle colour will change to show the player which attack is coming
    private void SelectColour()
    {
        Instantiate(swordParticle, swordLocation);
        switch (storedNumber[0])
        {
            case 0: LoopChildren(Color.red); break;
            case 1: LoopChildren(Color.green); break;
            case 2: LoopChildren(Color.blue); break;
        }
       
    }
    

    private void LoopChildren(Color colour)
    {
        foreach (ParticleSystem child in swordParticle.GetComponentsInChildren<ParticleSystem>())
        { 
            var main = child.main;
            main.startColor = colour;
        }
    }


    // Player must block correctly to avoid damage
    private void DamagePlayer()
    {
    
        switch (oldNumber)
        {
            case 0: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(25); break;
            case 1: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(25); break;
            case 2: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(25); break;
        }
    }









    // Decides whether to attack or chase depending on where player is
    private void CheckPlayerRange()
    {
        Debug.Log($"Distance: {distanceToPlayer}");
        animator.SetFloat("PlayerDistance", distanceToPlayer);
        canAttack = true;

    }

}
