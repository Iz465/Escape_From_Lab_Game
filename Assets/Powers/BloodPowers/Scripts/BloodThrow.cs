using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodThrow : BasePower, ICollide
{
    private BloodPowerData bloodData;
    private float radius = 100f;
    [SerializeField]
    private LayerMask enemyLayer;
    private Collider[] enemyDetected;
    private Collider powerCollider;

    public List<Collider> enemyHit = new List<Collider>();
    
    Vector3 direction;
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;
        powerCollider = GetComponent<Collider>();

    }


    // this ability will explode when collides
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
        if (enemyDetected.Length <= 0) return;

        Collider target = null;

        foreach (var enemy in enemyDetected)
        {
            if (!enemyHit.Contains(enemy))
            {
                target = enemy; break;
            }
        }
      
        if (target == null)
        {
    
            foreach (var enemy in enemyHit)
                if (enemy)
                    Physics.IgnoreCollision(enemy, powerCollider, false);
            enemyHit.Clear();
            poolManager.ReleaseToPool(powerData.prefab, gameObject);
            return;
        }
       
        direction = (target.transform.position - power.transform.position).normalized;
         
        Physics.IgnoreCollision(powerCollider, objectHit);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction * powerData.speed, ForceMode.Impulse);

    }

 
    


}



