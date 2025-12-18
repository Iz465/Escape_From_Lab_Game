using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Physics.Math;


public class ArcSwing : BasePower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask pillarLayer;
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

    public static GameObject[] enemyHit = new GameObject[1];
    [HideInInspector] public bool attackDisabled = false;

    public override void StartAttack(InputAction.CallbackContext context)
    {

   

        if (context.canceled)
        {
            heldDown = false;
            return;
        }


        if (context.performed)
        {
            if (attackDisabled) return;
            heldDown = true;
            StartCombo();
        
        }

   



    }

    private bool canHitPillar = true;

    [SerializeField] private float enemyDashDistance = 35;
    [SerializeField] private List<Transform> castPoints = new List<Transform>();


    private void StartCombo()
    {
        if (attackDisabled) return;

        RaycastHit enemyOutHit;
        RaycastHit pillarHit;


      

        if (canCombo)
            number++;

        int combinedMask = enemyLayer | wallLayer;


        bool castFound = false;
        for (int i = 0; i < castPoints.Count; i++)
        {
            if (castFound) break;

            bool hitPillar = Physics.Linecast(castPoints[i].position, castPoints[i].position + cam.transform.forward * 50, out pillarHit, pillarLayer);
            bool hitEnemy = Physics.Linecast(castPoints[i].position, castPoints[i].position + cam.transform.forward * enemyDashDistance, out enemyOutHit, combinedMask);


            if (!hitEnemy)
            {
                hittableObject = null;
                if (!hitPillar)
                {
                    castFound = false;
                    break;
                }
             
            }

            castFound = true;

            if (!hitPillar)
            {
                enemyDashDistance = 35;
                if (enemyHit[0] == enemyOutHit.collider.gameObject || enemyHit[0] == null)
                {
                    enemyHit[0] = enemyOutHit.collider.gameObject;
                    hittableObject = enemyOutHit.collider.gameObject;
                }



                else
                {
                    Debug.Log(enemyHit[0]);
                    hittableObject = null;
                }
            }


            if (hitPillar)
            {

                if (!canHitPillar) return;
                enemyDashDistance = 50;
                hittableObject = pillarHit.collider.gameObject;
                canCombo = false;
             
                animator.SetTrigger("Kick");
                canHitPillar = false;
                Player.canDamage = false;
                StartCoroutine(ResetPillarHit(1f));

                Move move = player.GetComponent<Move>();
                if (!move) return;
                move.fallSpeed = 0;
                move.fallAcceleration = 0.005f;
                if (resetCoroutine != null) StopCoroutine(resetCoroutine);
                StartCoroutine(ResetFallAcceleration(move, .3f));

            }


            // for breakable walls and enemies being targeted
            else
            {
                MeleeHitDetection.damage = stats.damage;
                BreakableWall.canHitWall = true;

                if (hittableObject != null)
                {
                    navmeshtestscript enemy = hittableObject.gameObject.GetComponent<navmeshtestscript>();
                    if (enemy) { if (!enemy.playerCanDash) return; }
                }

                canCombo = true;
                animator.SetTrigger("Arc Swing");

            }


        }






    }

    /*
                 Move move = player.GetComponent<Move>();
            if (!move) return;
            move.fallSpeed = 0;
            move.fallAcceleration = 0.005f;
            if (resetCoroutine != null) StopCoroutine(resetCoroutine);
            StartCoroutine(ResetFallAcceleration(move, 1));
     */

    private IEnumerator ResetPillarHit(float time)
    {
        yield return new WaitForSeconds(time);
        canHitPillar = true;
    }


    private void AllowCombo()
    {
    //    canCombo = true;
    }

    private void UnallowCombo()
    {
     //   canCombo = false;
    }

    private Coroutine resetCoroutine;
    private IEnumerator TravelToEnemy(float timer)
    {

        if (attackDisabled) yield break;
        if (!hittableObject) yield break;
        navmeshtestscript enemy = hittableObject.gameObject.GetComponent<navmeshtestscript>();
        if (enemy) if (!enemy.playerCanDash) yield break;

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


        Player.canDamage = true;

    }


    private IEnumerator ResetFallAcceleration(Move move, float time)
    {
        yield return new WaitForSeconds(time);
        move.fallAcceleration = 1f;
    }


    private CameraShake cameraShake;
    private void CanArcSwipe()
    {
        MeleeHitDetection.enemiesHit.Clear();
        MeleeHitDetection.canTrigger = true;

        if (number > 0)
        {
            if (attackDisabled)
            {
                number = 0;
                return;
            } 
            animator.SetBool("CanCombo", true);
        }
         

        else
            animator.SetBool("CanCombo", false);
        number = 0;

      
 
    }

    private void StartCameraShake()
    {
        if (attackDisabled) return;
        cameraShake = cam.GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.3f, 0.3f, 0.1f));
    }

    private void CombatStateEntered()
    {
        axeTransform.localPosition = axePositions.combatPosition;
        axeTransform.localRotation = axePositions.combatRotation;
        animator.SetBool("NotAttacking", false);
        if (attackDisabled) return;
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
        if (heldDown && !attackDisabled)
            StartCombo();
        MeleeHitDetection.canTrigger = false;
    }

    private void OnDrawGizmos()
    {
 
        Gizmos.color = Color.red;

        for (int i = 0; i < castPoints.Count; i++)
            Gizmos.DrawLine(castPoints[i].position, castPoints[i].position +  cam.transform.forward * 35);

    }





}
