using UnityEngine;
using UnityEngine.Rendering;

public class BlackKnight : navmeshtestscript
{
    [SerializeField] private ParticleSystem swordParticle;
    [SerializeField] private Transform swordLocation;
    private int[] storedNumber = new int[1];

  


    protected override void AttackPlayer()
    {
        canAttack = false;
        animator.SetTrigger("Combo");
    }

    private void SwitchAttackType()
    {

       

        int oldNumber = storedNumber[0];

        while (oldNumber == storedNumber[0])
        {
            int randomNumber = Random.Range(0, 3);
            storedNumber[0] = randomNumber;
        }

        Instantiate(swordParticle, swordLocation);
        switch(storedNumber[0])
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


}
