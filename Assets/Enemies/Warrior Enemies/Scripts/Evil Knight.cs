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

    [SerializeField] private float combatRadius; // the range at which enemies stop and stare while only one enemy goes into the attack range to attack
    private static EvilKnight[] knightAttacking = new EvilKnight[1];

    private Coroutine tauntCoroutine;

    // Enemy randomly chooses an attack

    protected override void Start()
    {
        base.Start();
        GlobalEnemyManager.totalEvilKnights.Add(gameObject);
        randomNumber = Random.Range(0, 3);
        agent.updateRotation = false;
        agent.speed = 8;
        animator.SetBool("ChosenEnemy", true);

    }

    private bool enteredRange = false;
    private bool checkEnteredRangeOnce = false;
    private bool disableMovement = true;
    protected override void ChasePlayer()
    {
        FacePlayer();

        if (distanceToPlayer > attackRange && canAttack && disableMovement)
        {
            if (!agent.isOnNavMesh) return;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            GlobalEnemyManager.enemiesInRange.Remove(gameObject);

        }


        if (distanceToPlayer <= attackRange)
        {
            enteredRange = true;
            checkEnteredRangeOnce = true;

            GlobalEnemyManager.enemiesInRange.Add(gameObject);


            if (agent.isOnNavMesh)
                agent.isStopped = true;

            if (canAttack)
                AttackPlayer();

        }

        if (distanceToPlayer >= combatRadius)
        {
            enteredRange = false;
        }



        if (!enteredRange)
        {
            if (checkEnteredRangeOnce)
            {
                
                agent.isStopped = true;
                checkEnteredRangeOnce = false;
                disableMovement = false;
                enemiesMovementDisabledAmount++;

                float time = 0;
                for (int i = 0; i < enemiesMovementDisabledAmount; i++)
                {
                    time += 3;
                    if (enemiesMovementDisabledAmount == 0) time = 0;
                }

                

                StartCoroutine(ReEnableMovement(time));
            }
       
        }




    
        
    }

    static int enemiesMovementDisabledAmount = 0;

    private IEnumerator ReEnableMovement (float time)
    {
        yield return new WaitForSeconds(time);
        disableMovement = true;
        enemiesMovementDisabledAmount--;


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
        yield return null;
        /*
        float time = 0;
        Vector3 originalPosition = transform.position;
        Vector3 endPosition = originalPosition + transform.forward * distance;
        while (time < timer)
        {
            transform.position = Vector3.Lerp(originalPosition, endPosition, time / timer);
            time += Time.deltaTime;
            yield return null;
        } */
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
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(15);


        if (randomNumber == 1)
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(15);

        if (randomNumber == 2)
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.blue) player.TakeDamage(15);

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




    private IEnumerator ActivateTaunt(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Taunt");
    }
    
}


