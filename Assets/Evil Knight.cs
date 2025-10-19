using System.Collections;
using UnityEngine;

public class EvilKnight : navmeshtestscript
{
    [Header("Magic Details")]
    [SerializeField] private ParticleSystem magicAttack;
    [SerializeField] private GameObject magicCast;
    [SerializeField] private Transform castLocation;
    [SerializeField] private LayerMask playerLayer;

    [Header("HitBox Positions")]
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
    [SerializeField] private Transform point4;
    [SerializeField] private Transform point5;
    [SerializeField] private Transform point6;
    [SerializeField] private Transform point7;
    [SerializeField] private Transform point8;
    [SerializeField] private Transform point9;
    [SerializeField] private Transform point10;

    private bool canHit = false;
    protected override void AttackPlayer()
    {
        canAttack = false;
        rotateSpeed = 20;
        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0) animator.SetTrigger("Swipe");
        if (randomNumber == 1)
        {
          //  if (distanceToPlayer <= 2.5f)
            StartCoroutine(StepDistance(0.5f));
            animator.SetTrigger("Down Attack");
        } 
             
    }

    protected override void Update()
    {
        base.Update();

        if (!canHit) return;
        bool checkPlayer1 = Physics.Linecast(point1.position, point2.position, playerLayer);
        bool checkPlayer2 = Physics.Linecast(point3.position, point4.position, playerLayer);
        bool checkPlayer3 = Physics.Linecast(point5.position, point6.position, playerLayer);
        bool checkPlayer4 = Physics.Linecast(point7.position, point8.position, playerLayer);
        bool checkPlayer5 = Physics.Linecast(point9.position, point10.position, playerLayer);

        if (checkPlayer1 || checkPlayer2 || checkPlayer3 || checkPlayer4 || checkPlayer5)
        {
            player.TakeDamage(20);
            canHit = false;
        }
      
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
        Gizmos.DrawLine(point3.position, point4.position);
        Gizmos.DrawLine(point5.position, point6.position);
        Gizmos.DrawLine(point7.position, point8.position);
        Gizmos.DrawLine(point9.position, point10.position);

    }

    private IEnumerator StepDistance(float timer)
    {

        float time = 0;
        Vector3 originalPosition = transform.position;
        Vector3 endPosition = originalPosition + transform.forward * 1f;
        while (time < timer)
        {
            transform.position = Vector3.Lerp(originalPosition, endPosition, time / timer);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetAnim()
    {
        canRotate = true;
        rotateSpeed = 5;
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

    private void EnableHit()
    {
        canRotate = false;
        canHit = true;
    }

    private void DisableHit()
    {
        canHit = false;
    }
}
