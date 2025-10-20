using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Physics.Math;

public class ArcSwing : BasePower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Collider playerCollider;
  
    public override void StartAttack(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        bool hitEnemy = Physics.Linecast(cam.transform.position, cam.transform.position + cam.transform.forward * 25, out hit, enemyLayer);
        if (!hitEnemy) return;

        GameObject enemy = hit.collider.gameObject;
        StartCoroutine(TravelToEnemy(0.5f, enemy));


        MeleeHitDetection.damage = stats.damage;
        base.StartAttack(context);
        Attack();
    }


    private IEnumerator TravelToEnemy(float timer, GameObject enemy)
    {
        float time = 0;
        CharacterController controller = playerCollider.GetComponent<CharacterController>();

        Vector3 startLocation = controller.gameObject.transform.position;
        Vector3 enemyLocation = enemy.transform.position;
        Vector3 enemyDirection = (enemyLocation - startLocation).normalized;
        float stopDistance = 4f;
        Vector3 stopLocation = enemyLocation - enemyDirection * stopDistance;

        Vector3 dashVector = stopLocation - startLocation;

        float distanceToEnemy = Vector3.Distance(startLocation, enemyLocation);
        if (distanceToEnemy <= 6) yield break;

        while (time < timer)
        {

            float t = time / timer;
          
            float smoothTime = Mathf.SmoothStep(0f, 1f, t);

            Vector3 nextPosition = startLocation + dashVector * smoothTime;

            controller.Move(nextPosition - controller.transform.position);
            
            time += Time.deltaTime;
            yield return null;
        }

    }

    private void CanArcSwipe()
    {
        MeleeHitDetection.canTrigger = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * 25);
  
    }



}
