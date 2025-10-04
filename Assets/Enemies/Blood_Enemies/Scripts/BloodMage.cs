using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class BloodMage : BloodEnemy
{
    [SerializeField] private GameObject power;
    [SerializeField] private float speed;
    [SerializeField] private Transform aimLoc;
    [SerializeField] private Collider meleeBox;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private LayerMask playerLayer;

    int number;
    private bool attack;
    private bool beam;
    private float beamLength;
    private GameObject beamInstance;
    
    protected override void Start()
    {
        base.Start();
        number = 0;
        beamLength = 20f;
    //    meleeBox.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (beam && beamInstance)
            ScaleBeam();
    }


    protected override void AttackPlayer()
    {
        agent.isStopped = true;

        int num = Random.Range(0, 2);

        if (num == 0)
        {
            attack = true;
            beam = false;
        }

        else if (num == 1)
        {
            beam = true;
            attack = false;
        }

        if (attack)
            animator.SetBool("CanAttack", true);
        else
            animator.SetBool("Beam", true);

        canAttack = false;
    }



    protected override void Attack()
    {
        number++;
        if (distanceToPlayer >= 10) speed = 80;
        if (distanceToPlayer < 10) speed = 50;
  
        GameObject powerInstance = Instantiate(power, aimLoc.position, transform.rotation);
        if (!powerInstance) return;
        Rigidbody rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDir = (collider.bounds.center - aimLoc.position).normalized;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(aimDir * speed, ForceMode.Impulse);
         
    }




    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);
        if (number >= 3)
            StartCoroutine(ResetAttack(1.5f));
        else
            StartCoroutine(ResetAttack(.1f));
                
        //     meleeBox.enabled=false;
    }


    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds (time);
       
        if (number >= 3)
        {
            number = 0;
            canAttack = true;
        }
           
        else
            animator.SetBool("CanAttack", true);

    }

    private void BeamAttack()
    {
        rotateSpeed = 3f;
        number = 3;
        beamInstance = Instantiate(beamPrefab, aimLoc.position, transform.rotation);
        beamInstance.transform.SetParent(aimLoc, false);
        beamInstance.transform.localScale = new Vector3(0.01f, 0.01f, 0.02650874f);
        beamInstance.transform.localPosition = Vector3.zero;
        beamInstance.transform.localRotation = Quaternion.identity;
        StartCoroutine(ResetBeam(beamInstance, 5));
    }



    private void ScaleBeam()
    {
        RaycastHit hit;
     

        if (Physics.Raycast(beamInstance.transform.parent.position, beamInstance.transform.parent.forward, out hit, beamLength, playerLayer))
        {
            Debug.DrawRay(beamInstance.transform.parent.position, beamInstance.transform.parent.forward * beamLength, Color.cyan);
            beamLength = hit.distance;
        }
    

        else
            beamLength = Mathf.Lerp(beamLength, 20f, Time.deltaTime / 1f);


        var scale = beamInstance.transform.parent.localScale;
        scale.z = beamLength * 1.55f; // bandaid solution.
        beamInstance.transform.parent.localScale = scale;

    }

    private IEnumerator ResetBeam(GameObject beamInstance, int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Beam", false);
        Destroy(beamInstance);
        StartCoroutine(ResetAttack(1.5f));
        rotateSpeed = 5f;

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


    