using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// this ability will insta kill all enemies under 50% hp. also ricochets to another enemy if it hits an enemy.
public class InstaKill : BasePower, ICollide
{
    
    private float radius = 100f;
    [SerializeField]
    private LayerMask enemyLayer;
    private Collider[] enemyDetected;
    private Collider powerCollider;
    private bool isHeldDown;
    [SerializeField] public AudioClip attackSound;


    private List<GameObject> enemyHit = new List<GameObject>();
    private CameraShake cameraShake;

    private void Awake()
    {
   
        powerCollider = GetComponent<Collider>();
      
    }

    // Checks every frame whether player is holding down mouse or not. This is so the player can spam the power without having to click many times


    private void Update()
    {
      //  if (animator)
      //      animator.SetBool("Continued", isHeldDown);
        if (Input.GetMouseButton(0))
        {
        
            isHeldDown = true;
        }
         
        if (Input.GetMouseButtonUp(0))
        {
    
            isHeldDown = false;
        }
        
    }


    public override void StartAttack(InputAction.CallbackContext context)
    {
       
        if (!context.started) return;
        animator.SetTrigger(stats.powerName);


    }


    // animation event called during the part where the animation forms an attack
    private void StartInstaKill()
    {
        if (attackSound) player.audioSource.PlayOneShot(attackSound, 3f);
        
        if (!cam) return;
        cameraShake = cam.GetComponent<CameraShake>();
        if (!cameraShake) return;
        StartCoroutine(cameraShake.Shake(0.1f, 0.1f, 0.1f));
        Attack();
    }


    // Power either destroys on enemy or ricochets depending on enemy amount.
    public void CollideResult(Collider objectHit, GameObject power)
    {
        Debug.Log("Activating insta kill");
        Debug.Log($"Object hit : {objectHit}");
        if (!powerCollider) return;
        rb = power.GetComponent<Rigidbody>();
        if (!rb) return;
        if (!enemyHit.Contains(objectHit.gameObject)) enemyHit.Add(objectHit.gameObject);
         
        enemyDetected = Physics.OverlapSphere(power.transform.position, radius, enemyLayer);
       

        Collider target = null;

 
        foreach (var enemy in enemyDetected)
        {
            if (!enemyHit.Contains(enemy.gameObject))
            {
                target = enemy; break;
            }
            
        }
        Debug.Log($"Target: {target}");

        if (!target)
        {
            foreach (var enemy in enemyHit)
                if (enemy)
                    Physics.IgnoreCollision(objectHit, powerCollider, false);
            enemyHit.Clear();
            poolManager = FindFirstObjectByType<ObjectPoolManager>(); // temporary. 
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        Vector3 direction = (target.bounds.center - power.transform.position).normalized;
 

        Physics.IgnoreCollision(powerCollider, objectHit);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction * stats.speed, ForceMode.Impulse);
      
    }


    // These are bandaid fixes required to show the animation event is here. will remove them soon.
    private void CombatStateEntered()
    {

    }

    private void NonCombatStateEntered()
    {

    }
}
