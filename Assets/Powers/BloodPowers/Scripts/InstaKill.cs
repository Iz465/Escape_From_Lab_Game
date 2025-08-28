using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this ability will insta kill all enemies under 50% hp. also ricochets to another enemy if it hits an enemy.
public class InstaKill : BasePower, ICollide
{
    BloodPowerData bloodData;
    private float radius = 100f;
    [SerializeField]
    private LayerMask enemyLayer;
    private Collider[] enemyDetected;
    private Collider powerCollider;

    public List<Collider> enemyHit = new List<Collider>();

    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;
        powerCollider = GetComponent<Collider>();
    }


    
    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        playerData.health -= bloodData.loseHealth;
        playerData.health = Mathf.Clamp(playerData.health, 10, playerData.maxHealth);


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
            poolManager.ReleaseToPool(powerData.prefab, gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - power.transform.position).normalized;

        Physics.IgnoreCollision(powerCollider, objectHit);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction * powerData.speed, ForceMode.Impulse);

    }

}
