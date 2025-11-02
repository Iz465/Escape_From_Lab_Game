using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Physics.Math;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ArcSwing : BasePower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private Transform axeTransform;
    [SerializeField] private AudioClip hammerSwingSound;
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
    private GameObject hittableObject;
    private bool heldDown;

    private GameObject[] enemyHit = new GameObject[1];

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

        int combinedMask = enemyLayer | wallLayer;



        bool hitEnemy = Physics.Linecast(playerCollider.bounds.center, cam.transform.position + cam.transform.forward * 25, out hit, combinedMask);

        if (!hitEnemy)
        {
            hittableObject = null;
            return;
        }
    
        if (enemyHit[0] == hit.collider.gameObject || enemyHit[0] == null)
        {
            enemyHit[0] = hit.collider.gameObject;
            hittableObject = hit.collider.gameObject;
        }


        else
        {
            Debug.Log(enemyHit[0]);
            hittableObject = null;
        }
           



        MeleeHitDetection.damage = stats.damage;
        BreakableWall.canHitWall = true;
        animator.SetTrigger("Arc Swing");
        canCombo = true;

       
    }



    private IEnumerator TravelToEnemy(float timer)
    {
        if (!hittableObject) yield break;
        float time = 0;
        CharacterController controller = playerCollider.GetComponent<CharacterController>();

        Vector3 startLocation = controller.gameObject.transform.position;
        Vector3 hittableObjectLocation = hittableObject.transform.position;
        Vector3 hittableObjectDirection = (hittableObjectLocation - startLocation).normalized;
        float stopDistance = 4f;
        Vector3 stopLocation = hittableObjectLocation - hittableObjectDirection * stopDistance;

        Vector3 dashVector = stopLocation - startLocation;

        float distanceToEnemy = Vector3.Distance(startLocation, hittableObjectLocation);
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
        MeleeHitDetection.enemiesHit.Clear();
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
        StartCoroutine(cameraShake.Shake(0.3f, 0.3f, 0.1f));
    }

    private void CombatStateEntered()
    {
        axeTransform.localPosition = axePositions.combatPosition;
        axeTransform.localRotation = axePositions.combatRotation;
        animator.SetBool("NotAttacking", false);

        if (player.audioSource) player.audioSource.PlayOneShot(hammerSwingSound);
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
        MeleeHitDetection.canTrigger = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
        Gizmos.DrawLine(playerCollider.bounds.center, cam.transform.position + cam.transform.forward * 25);
  
    }





}
