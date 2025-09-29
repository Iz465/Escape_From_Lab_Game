using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BloodMage : BloodEnemy
{
    [SerializeField]
    private GameObject power;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform aimLoc;
    [SerializeField]
    private Collider meleeBox;

    protected override void Start()
    {
        base.Start();
    //    meleeBox.enabled = false;
    }





    protected override void Attack()
    {
  
        GameObject powerInstance = Instantiate(power, aimLoc.position, transform.rotation);
        if (!powerInstance) return;
        Rigidbody rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDir = (collider.bounds.center - aimLoc.position).normalized;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(aimDir * speed * 100 * Time.deltaTime, ForceMode.Impulse);



    }

  

    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);

   //     meleeBox.enabled=false;



        StartCoroutine(ResetAttack(3f));                        


    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds (time);
        canAttack = true;

    }



    private void Melee()

    {
        Debug.Log("Melee");
 //       meleeBox.enabled=true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player playerHit = other.GetComponent<Player>();
        Debug.Log($"Other : {other}");
        if (meleeBox.enabled == true && playerHit)
        {
            Debug.Log("Melee Has overlapped!");
            player.TakeDamage(20);
        }
    
    }


}


    