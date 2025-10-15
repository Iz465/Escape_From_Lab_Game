using System.Collections;
using UnityEngine;

public class EvilKnight : navmeshtestscript
{
    [Header("Magic Details")]
    [SerializeField] private ParticleSystem magicAttack;
    [SerializeField] private GameObject magicCast;
    [SerializeField] private Transform castLocation;
 

    protected override void AttackPlayer()
    {
        canAttack = false;
        animator.SetTrigger("Slash");
    }

    private void ResetAnim()
    {
        StartCoroutine(CanAttack(2));
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
}
