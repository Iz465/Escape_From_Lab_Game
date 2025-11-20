using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class EvilKnight : navmeshtestscript
{
    [Header("Magic Details")]
    [SerializeField] private Transform castLocation;



    [Header("Attack Types")]
    [SerializeField] private ParticleSystem redAttack;
    [SerializeField] private ParticleSystem greenAttack;
    [SerializeField] private ParticleSystem blueAttack;

    [Header("Sounds")]
    [SerializeField] private AudioClip firstStepSound;
    [SerializeField] private AudioClip secondStepSound;
    [SerializeField] private AudioClip swordHitSound;
    
    private bool canHit = false;
    private int randomNumber;

    // Enemy randomly chooses an attack

    protected override void Start()
    {
        base.Start();
        GlobalEnemyManager.totalEvilKnights.Add(gameObject);
        randomNumber = Random.Range(0, 3);

    }

    private bool firstTime = true;
    protected override void AttackPlayer()
    {
  

        canAttack = false;
        rotateSpeed = 20;
   
        int oldNumber = randomNumber;

        while (oldNumber == randomNumber)
        {
            randomNumber = Random.Range(0, 3);
        }

        if (firstTime)
        {
            randomNumber = 0;
            firstTime = false;
        } 

        if (randomNumber == 0)
        {
            Instantiate(redAttack, castLocation);
            animator.SetTrigger("Down Attack");
        }

        if (randomNumber == 1)
        {
            Instantiate(greenAttack, castLocation); ;
            StartCoroutine(StepDistance(0.5f, 1f));
            animator.SetTrigger("Down Attack");
        }
        

        if (randomNumber == 2)
        {
            Instantiate(blueAttack, castLocation);
            animator.SetTrigger("Down Attack");
        }


    }

    // Added this for enemy to glide towards player during a specific attack and look more natural
    private IEnumerator StepDistance(float timer, float distance)
    {

        float time = 0;
        Vector3 originalPosition = transform.position;
        Vector3 endPosition = originalPosition + transform.forward * distance;
        while (time < timer)
        {
            transform.position = Vector3.Lerp(originalPosition, endPosition, time / timer);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetAnim()
    {
        rotateSpeed = 5;
        StartCoroutine(CanAttack(0f));
    }



    private IEnumerator CanAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    
    }


    
    // The window during the enemies attack animation that allows them to damage the player.
    private void EnableHit()
    {
        globalEnemyManager.CheckEnemySound(attackSound, "attack", audioSource);

        if (randomNumber == 0)
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(15 * GlobalEnemyManager.damageMultiplier);


        if (randomNumber == 1)
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(15 * GlobalEnemyManager.damageMultiplier);

        if (randomNumber == 2)
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(15 * GlobalEnemyManager.damageMultiplier);

        canHit = true;
    }


   
    private void DisableHit()
    {
        audioSource.PlayOneShot(swordHitSound, 3f);
        canHit = false;


    }

    private void FirstKnightStep()
    {
        globalEnemyManager.CheckEnemySound(firstStepSound, "footsteps", audioSource);
    }

    private void SecondKnightStep()
    {
        globalEnemyManager.CheckEnemySound(secondStepSound, "footsteps", audioSource);
    }


    protected override void EnemyDeath()
    {

        base.EnemyDeath();
        globalEnemyManager.RespawnEnemyWave(GlobalEnemyManager.totalEvilKnights, gameObject);

    }

    
}


