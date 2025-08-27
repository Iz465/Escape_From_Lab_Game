using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BloodThrow : BasePower, ICollide
{
    BloodPowerData bloodData;
    private float radius = 100f;
    [SerializeField]
    private LayerMask enemyLayer;
    private Collider[] enemyDetected;
    
    Vector3 direction;
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;

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
        
        Collider collider = power.GetComponent<Collider>();
        if (!collider) return;
        Debug.Log("Entered collide result");
        rb = power.GetComponent<Rigidbody>();
        if (!rb) return;
        enemyDetected = Physics.OverlapSphere(power.transform.position, radius, enemyLayer);
        if (enemyDetected.Length <= 0) return;
  
        if (enemyDetected[0] == objectHit)
        {
            if (enemyDetected.Length <= 1) return;
       //     Physics.IgnoreCollision(collider, enemyDetected[0], false);
            direction = (enemyDetected[1].transform.position - power.transform.position).normalized;
            Physics.IgnoreCollision(collider, enemyDetected[0]);  
        }   

       
         
        else
        {
        //    Physics.IgnoreCollision(collider, enemyDetected[1], false);
            direction = (enemyDetected[0].transform.position - power.transform.position).normalized;
            if (enemyDetected.Length > 1) 
                Physics.IgnoreCollision(collider, enemyDetected[1]);
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(direction * powerData.speed, ForceMode.Impulse);
        

    }

 
    


}



