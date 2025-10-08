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
    [SerializeField] private GameObject instantAttackPrefab;
    [SerializeField] private LayerMask playerLayer;

    int number;
    private bool attack;
    private bool beam;
    private bool instantAttack;
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

        int num = Random.Range(0, 10);

  
        switch (num)
        {
            case >= 0 and <= 4: attack = true; beam = false; instantAttack = false; break;
            case >= 5 and <= 8: attack = false; beam = true; instantAttack = false; break;
            case 9: attack = false; beam = false; instantAttack = true; break;
        }

        if (attack)
            animator.SetBool("CanAttack", true);
        else if (beam)
            animator.SetBool("Beam", true);
        else if (instantAttack)
            InstantAttack();

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
            StartCoroutine(ResetAttack(1f));
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
        StartCoroutine(ResetBeam(beamInstance, 3));
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
        StartCoroutine(ResetAttack(1f));
        rotateSpeed = 5f;

    }

    private void InstantAttack()
    {
        animator.SetBool("InstantAttack", true);
        number = 3;

        GameObject instant = Instantiate(instantAttackPrefab, transform.position + new Vector3(0, 8, 0), transform.rotation);
        StartCoroutine(HitPlayer(instant,10f));

        instant = Instantiate(instantAttackPrefab, transform.position + new Vector3(5, 8, 0), transform.rotation);
        StartCoroutine(HitPlayer(instant, 10f));

        instant = Instantiate(instantAttackPrefab, transform.position + new Vector3(-5, 8, 0), transform.rotation);
        StartCoroutine(HitPlayer(instant, 10f));

      
    }

    private void ResetInstantAttack(float time)
    {
        animator.SetBool("InstantAttack", false);
        StartCoroutine(ResetAttack(1f));
    }





    private IEnumerator HitPlayer(GameObject instant,float timer)
    {
        yield return new WaitForSeconds(timer);
        if (!instant) yield break;
        Rigidbody rb = instant.GetComponent<Rigidbody>();
        if (!rb) yield break;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDir = (collider.bounds.center - instant.transform.position).normalized;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(aimDir * 300, ForceMode.Impulse);
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


    