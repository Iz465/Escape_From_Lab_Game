using UnityEngine;
using UnityEngine.Rendering;

public class BlackKnight : navmeshtestscript
{
    [SerializeField] private ParticleSystem swordParticle;
    [SerializeField] private Transform swordLocation;
    private int[] storedNumber = new int[1];
    int oldNumber;
    bool doOnce = true;
  


    protected override void AttackPlayer()
    {
        canAttack = false;
        animator.SetTrigger("Combo");
    }

    private void SwitchAttackType()
    {
        


        oldNumber = storedNumber[0];



        while (oldNumber == storedNumber[0])
        {
            int randomNumber = Random.Range(0, 3);
            storedNumber[0] = randomNumber;
        }


        if (doOnce)
        {
            Instantiate(swordParticle, swordLocation);
            switch (storedNumber[0])
            {
                case 0: LoopChildren(Color.red); break;
                case 1: LoopChildren(Color.green); break;
                case 2: LoopChildren(Color.blue); break;
            }

            oldNumber = storedNumber[0];
            doOnce = false;
        }

        Instantiate(swordParticle, swordLocation);
        switch(storedNumber[0])
        {
            case 0: LoopChildren(Color.red); break;
            case 1: LoopChildren(Color.green); break;
            case 2: LoopChildren(Color.blue); break;
        }
      
      
      //  Debug.Log($"Before : {oldNumber}");
    }

    

    private void LoopChildren(Color colour)
    {
        foreach (ParticleSystem child in swordParticle.GetComponentsInChildren<ParticleSystem>())
        { 
            var main = child.main;
            main.startColor = colour;
        }
    }

    private void DamagePlayer()
    {
      //  Debug.Log($"After : {oldNumber}");
      
        
        switch (oldNumber)
        {
            case 0: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(25); break;


            case 1: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(25); break;

            case 2: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(25); break;

        }
    }










    private void CheckPlayerRange()
    {
        Debug.Log($"Distance: {distanceToPlayer}");
        animator.SetFloat("PlayerDistance", distanceToPlayer);
        canAttack = true;

    }

}
