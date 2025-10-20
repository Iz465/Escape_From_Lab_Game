using System.Collections;
using UnityEngine;

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
        StartCoroutine(CanAttack(0.5f));
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

    private void CastMagic()
    {
        summonMagic.Stop();
        GameObject magicCastInstance = Instantiate(magicCast, castLocation.position + new Vector3(0, 0.5f, 0), transform.rotation);
        Rigidbody body = magicCastInstance.GetComponent<Rigidbody>();
        if (!body) return;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDirection = (collider.bounds.center - castLocation.position).normalized;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.AddForce(aimDirection * 100, ForceMode.Impulse);
      
    }

    private void EnableHit()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Swipe"))
        {
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.red) player.TakeDamage(35);
        }
           
        else if (state.IsName("Down Attack"))
        {
            if (BlockAttacks.particleInUse != BlockAttacks.ParticleInUse.green) player.TakeDamage(35);
        }
         

                canHit = true;
    }

    private void DisableHit()
    {
        canHit = false;
    }


}


