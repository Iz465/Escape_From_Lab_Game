using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Physics.Math;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ArcSwing : BasePower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private Transform axeTransform;

    [System.Serializable] public struct AxePositions 
    {
        [SerializeField] public Vector3 idlePosition;
        [SerializeField] public Quaternion idleRotation;
        [SerializeField] public Vector3 combatPosition;
        [SerializeField] public Quaternion combatRotation;
    }
    public AxePositions axePositions;

    private int number;
    private bool canCombo = false;
    private GameObject enemy;
    private bool heldDown;

   
    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            heldDown = true;
            StartCombo();
        
        }

        if (context.canceled)
            heldDown = false;



    }

    private void StartCombo()
    {
        RaycastHit hit;

        if (canCombo)
            number++;


        bool hitEnemy = Physics.Linecast(cam.transform.position, cam.transform.position + cam.transform.forward * 25, out hit, enemyLayer);
        if (!hitEnemy) return; //{ animator.SetBool("CanCombo", false);  return; }

        enemy = hit.collider.gameObject;

        MeleeHitDetection.damage = stats.damage;
        animator.SetTrigger("Arc Swing");
        Attack();
        canCombo = true;
        player.playerHitParticle = hitParticle;
    }



    private IEnumerator TravelToEnemy(float timer)
    {
        if (!enemy) yield break;
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

    private CameraShake cameraShake;
    private void CanArcSwipe()
    {
        MeleeHitDetection.canTrigger = true;

        if (number > 0)
            animator.SetBool("CanCombo", true);
        else
            animator.SetBool("CanCombo", false);
        number = 0;
 
    }

    private void StartCameraShake()
    {
        cameraShake = cam.GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.1f));
    }

    private void CombatStateEntered()
    {
        axeTransform.localPosition = axePositions.combatPosition;
        axeTransform.localRotation = axePositions.combatRotation;
        animator.SetBool("NotAttacking", false);
    }

    private void NonCombatStateEntered()
    {
        axeTransform.localPosition = axePositions.idlePosition;
        axeTransform.localRotation = axePositions.idleRotation;
        animator.SetBool("NotAttacking", true);
        animator.ResetTrigger(stats.powerName);
    }

    private void EndOfAttack()
    {
        if (heldDown)
            StartCombo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * 25);
  
    }





}
