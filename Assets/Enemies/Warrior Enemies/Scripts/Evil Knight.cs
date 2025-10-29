using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class EvilKnight : navmeshtestscript
{
    [Header("Magic Details")]
    [SerializeField] private ParticleSystem magicAttack;
    [SerializeField] private GameObject magicCast;
    [SerializeField] private Transform castLocation;
    [SerializeField] private LayerMask playerLayer;


    [Header("Attack Types")]
    [SerializeField] private ParticleSystem redAttack;
    [SerializeField] private ParticleSystem greenAttack;
    [SerializeField] private ParticleSystem blueAttack;


    private bool canHit = false;


    protected override void AttackPlayer()
    {
     //   if (!GlobalEnemyManager.globalCanAttack) return;

        canAttack = false;
        rotateSpeed = 20;

        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0)
        {
            Instantiate(redAttack, castLocation);
            animator.SetTrigger("Swipe");
        }

        if (randomNumber == 1)
        {
            Instantiate(greenAttack, castLocation); ;
            StartCoroutine(StepDistance(0.5f, 1f));
            animator.SetTrigger("Down Attack");
        }


    }

 

 

 

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

    private ParticleSystem summonMagic;
    private void SummonMagic()
    {
        summonMagic = Instantiate(magicAttack, castLocation);

    }

 
    

    private void EnableHit()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Swipe"))
        {
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(15);
        }
           
        else if (state.IsName("Down Attack"))
        {
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(15);
        }
         

                canHit = true;
    }

    private void DisableHit()
    {
        canHit = false;


    }


}


