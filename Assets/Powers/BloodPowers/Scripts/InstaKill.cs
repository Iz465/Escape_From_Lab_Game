using System.Collections.Generic;
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

    private List<Collider> enemyHit = new List<Collider>();

    private void Awake()
    {
   
        powerCollider = GetComponent<Collider>();
    }


    
    protected override bool UseStamina()
    {

     //   player.stats.health -= bloodData.loseHealth;
        player.stats.health = Mathf.Clamp(player.stats.health, 10, player.maxHealth);


        return base.UseStamina();
    }

    public void CollideResult(Collider objectHit, GameObject power)
    {

        if (!powerCollider) return;
        rb = power.GetComponent<Rigidbody>();
        if (!rb) return;
        if (!enemyHit.Contains(objectHit)) enemyHit.Add(objectHit);
        enemyDetected = Physics.OverlapSphere(power.transform.position, radius, enemyLayer);


        Collider target = null;


        foreach (var enemy in enemyDetected)
        {
            if (!enemyHit.Contains(enemy))
            {
                target = enemy; break;
            }
        }

        if (!target)
        {
         
            foreach (var enemy in enemyHit)
                if (enemy)
                    Physics.IgnoreCollision(enemy, powerCollider, false);
            enemyHit.Clear();
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - power.transform.position).normalized;

        Physics.IgnoreCollision(powerCollider, objectHit);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction * stats.speed, ForceMode.Impulse);

    }

}
