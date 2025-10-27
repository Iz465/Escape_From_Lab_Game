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



    private List<GameObject> enemyHit = new List<GameObject>();
    CameraShake cameraShake;

    private void Awake()
    {
   
        powerCollider = GetComponent<Collider>();
      
    }

    private void Update()
    {
        if (animator)
            animator.SetBool("Continued", isHeldDown);
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



    private void StartInstaKill()
    {
        if (!cam) return;
        cameraShake = cam.GetComponent<CameraShake>();
        if (!cameraShake) return;
        StartCoroutine(cameraShake.Shake(0.1f));
        Attack();
    }

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

    private void StartShake()
    {
        if (!cam) return;
        cameraShake = cam.GetComponent<CameraShake>();
        if (!cameraShake) return;
        StartCoroutine(cameraShake.Shake(1f));
    }

    private void ResetAnim()
    {
       
        
        
      
    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
  
    }


    private void CombatStateEntered()
    {

    }

    private void NonCombatStateEntered()
    {

    }
}
