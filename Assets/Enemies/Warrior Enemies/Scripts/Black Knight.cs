using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackKnight : navmeshtestscript
{
    [SerializeField] private ParticleSystem swordParticle;
    [SerializeField] private GameObject redParticleAttack;
    [SerializeField] private GameObject greenParticleAttack;
    [SerializeField] private GameObject blueParticleAttack;
    [SerializeField] private Transform particleAttackLocation;
    [SerializeField] private Transform swordLocation;
    private int[] storedNumber = new int[1];
    private List<GameObject> particleAttackPattern = new List<GameObject>();
    private int oldNumber;
    bool doOnce = true;


    [Header("Sounds")]
    [SerializeField] private AudioClip firstStepSound;
    [SerializeField] private AudioClip secondStepSound;
    [SerializeField] private AudioClip swordHitSound;
    


  
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
        ParticleSystem swordInstance = Instantiate(swordParticle, swordLocation);
        audioSource.PlayOneShot(attackSound);
        switch (storedNumber[0])
        {
            case 0: LoopChildren(swordInstance, Color.red); break;
            case 1: LoopChildren(swordInstance, Color.green); break;
            case 2: LoopChildren(swordInstance, Color.blue); break;
        } 
       
    }
    

    private void LoopChildren(ParticleSystem swordInstance, Color colour)
    {
        foreach (ParticleSystem child in swordInstance.GetComponentsInChildren<ParticleSystem>())
        { 
            var main = child.main;
            main.startColor = colour;
        }
    }


    // Player must block correctly to avoid damage
    private void DamagePlayer()
    {
        audioSource.PlayOneShot(swordHitSound, 3f);
        switch (oldNumber)
        {
            case 0: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(25); break;
            case 1: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(25); break;
            case 2: if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(25); break;
        }
    }

    // Switches cast attack types. Called via animation event
    private IEnumerator CastAttack(float attackTime)
    {

        animator.speed = 0;
        foreach (ParticleSystem child in swordParticle.GetComponentsInChildren<ParticleSystem>())
        {
            var main = child.main;
            main.duration = attackTime;
            main.startLifetime = attackTime;
        }

        List<int> numberCollection = new List<int>() {0, 1, 2};


        for (int i = 0; i < 3; i ++)
        {
            Debug.Log(numberCollection);
            ParticleSystem swordCombo = Instantiate(swordParticle, swordLocation);


            int randomIndex = Random.Range(0, numberCollection.Count);
            int randomValue = numberCollection[randomIndex];




            switch (randomValue)
            {
                case 0: LoopChildren(swordCombo, Color.red); particleAttackPattern.Add(redParticleAttack); break;
                case 1: LoopChildren(swordCombo, Color.green); particleAttackPattern.Add(greenParticleAttack); break;
                case 2: LoopChildren(swordCombo, Color.blue); particleAttackPattern.Add(blueParticleAttack); break;
            }

            // so no colour repeats
            numberCollection.RemoveAt(randomIndex);
 

            yield return new WaitForSeconds(attackTime);
            Destroy(swordCombo.gameObject);
        }

        animator.speed = 1;

    }

    private IEnumerator ShootParticle(float time)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject particleInstance = Instantiate(particleAttackPattern[i], particleAttackLocation.position, Quaternion.identity);
            Rigidbody rb = particleInstance.GetComponent<Rigidbody>();
            Collider collider = player.GetComponent<Collider>();
            Vector3 playerPosition = collider.bounds.center - particleAttackLocation.position;
            rb.AddForce(playerPosition * 20, ForceMode.Impulse);

      

            yield return new WaitForSeconds(time);
        }

        particleAttackPattern.Clear();
    }

    private IEnumerator ResetCast(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    


    private void FirstKnightStep()
    {
        globalEnemyManager.CheckEnemySound(firstStepSound, "footsteps", audioSource);
    }

    private void SecondKnightStep()
    {
        globalEnemyManager.CheckEnemySound(secondStepSound, "footsteps", audioSource);
    }






    // Decides whether to attack or chase depending on where player is
    private void CheckPlayerRange()
    {
        Debug.Log($"Distance: {distanceToPlayer}");
        animator.SetFloat("PlayerDistance", distanceToPlayer);
        canAttack = true;

    }

}
