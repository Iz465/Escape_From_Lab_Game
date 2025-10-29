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


    private bool canHit = false;

    // Enemy randomly chooses an attack
    protected override void AttackPlayer()
    {
  

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
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Swipe"))
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(15);


        if (state.IsName("Down Attack"))
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(15);

        canHit = true;
    }


   
    private void DisableHit()
    {
        canHit = false;


    }


}


