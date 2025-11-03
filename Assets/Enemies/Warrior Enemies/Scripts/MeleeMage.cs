using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using static UnityEngine.UI.Image;

public class MeleeMage : navmeshtestscript
{
    [SerializeField] private GameObject power;
    [SerializeField] private float powerSpeed;
    [SerializeField] private Transform aimLoc;
    [SerializeField] private LayerMask playerLayer;

    int number;



    protected override void Start()
    {
        base.Start();
        number = 0;
     
    
    }



    protected override void AttackPlayer()
    {
        canAttack = false;
        animator.SetBool("CanAttack", true);
    }


    protected override void Attack()
    {
     
  
        number++;
        
  
        GameObject powerInstance = Instantiate(power, aimLoc.position, transform.rotation);

        if (!powerInstance) return;
        Rigidbody rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDir = (collider.bounds.center - aimLoc.position).normalized;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(aimDir * powerSpeed, ForceMode.Impulse);

    }


    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);
        if (number >= 3)
            StartCoroutine(ResetAttack(1));

        else
            StartCoroutine(ResetAttack(.1f));
                
    }


    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds (time);
       
        
        if (number < 3)
            animator.SetBool("CanAttack", true);

        else
        {
            number = 0;
            canAttack = true;
        }

    }

 
    



}


    