using System.Collections.Generic;
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
    Animator animator;

    private List<GameObject> enemyHit = new List<GameObject>();

    private void Awake()
    {
   
        powerCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack(InputAction.CallbackContext context)
    {
        base.Attack(context);
        if (animator)
            animator.SetBool("InstaKill", true);
    }



    protected override bool UseStamina()
    {

     //   player.stats.health -= bloodData.loseHealth;
        player.stats.health = Mathf.Clamp(player.stats.health, 10, player.maxHealth);


        return base.UseStamina();
    }

    public void CollideResult(Collider objectHit, GameObject power)
    {
        Debug.Log("Activating insta kill");
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

    private void ResetAnim()
    {
        animator.SetBool("InstaKill", false);
    }
}
