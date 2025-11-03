using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using static UnityEngine.UI.Image;

public class BloodMage : BloodEnemy
{
    [SerializeField] private GameObject power;
    [SerializeField] private float speed;
    [SerializeField] private Transform aimLoc;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject circleInstantPrefab;
    [SerializeField] private GameObject instantAttackPrefab;
    [SerializeField] private LayerMask playerLayer;

    int number;
    private static bool attack;
    private bool beam;
    private static bool instantAttack;
    private float beamLength;
    private GameObject beamInstance;

    private static List<BloodMage> bloodMages = new List<BloodMage>();
    private static BloodMage[] mageInUse = new BloodMage[1];


    protected override void Start()
    {
        base.Start();
        number = 0;
        beamLength = 20f;
        bloodMages.Add(this);


    }

    protected override void Update()
    {
        base.Update();

        if (beam && beamInstance)
            ScaleBeam();
    }


    protected override void AttackPlayer()
    {
        if (agent.isOnNavMesh)
            agent.isStopped = true;

        canAttack = false;
     



        if (mageInUse[0] != null && mageInUse[0] != this)
        {
            animator.SetBool("Beam", true);
            beam = true;
            return;
        }

        if (mageInUse[0] == null)
        {
            int num = Random.Range(0, 2);
            switch (num)
            {
                case 0: attack = true; break;
                case 1: instantAttack = true; break;
            }
            mageInUse[0] = this;
        }

        if (mageInUse[0] == this)
        {

            if (attack)
                animator.SetBool("CanAttack", true);

            else if (instantAttack)
                animator.SetBool("InstantAttack", true);
        }


        else
        {
            beam = true;
            animator.SetBool("Beam", true);
        }
        



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
            StartCoroutine(ResetAttack(1));

        else
            StartCoroutine(ResetAttack(.1f));
                

    }


    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds (time);
       
        if (number >= 3)
        {
            if (mageInUse[0] == this)
            {
                instantAttack = false;
                attack = false;
                mageInUse[0] = null;
            }
        
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
            player.TakeDamage(30 * Time.deltaTime);

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
        beam = false;
        StartCoroutine(ResetAttack(1));
        rotateSpeed = 5f;

    }

    private Vector3 startingPosition;
    private GameObject circleAttackInstance;
    private void InstantAttack()
    {
        animator.SetBool("InstantAttack", true);
        number = 3;

        startingPosition = player.transform.position;
        StartCoroutine(HitPlayer(1f));
        circleAttackInstance = Instantiate(circleInstantPrefab, player.transform.position, Quaternion.identity);

        circleAttackInstance.transform.localScale = new Vector3(2, 2, 2);

    }


    private void ResetInstantAttack(float time)
    {
        animator.SetBool("InstantAttack", false);
        StartCoroutine(ResetAttack(2));
    }




    // The scale of instant attack must be three times smaller than circle attack scale so that both have same size.

    // overlap sphere radius 4 = 1,1,1 of circleAttackInstance.
    private IEnumerator HitPlayer(float timer)
    {
        yield return new WaitForSeconds(timer);
        GameObject instantAttackInstance = Instantiate(instantAttackPrefab, startingPosition, Quaternion.identity);

        instantAttackInstance.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        Destroy(circleAttackInstance);

        Collider[] playerCollider = Physics.OverlapSphere(startingPosition, 8, playerLayer);

        if (playerCollider.Length > 0)
            player.TakeDamage(35);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startingPosition, 8);
    }

    protected override void EnemyDeath()
    {
        bloodMages.Remove(this);


        if (mageInUse[0] == this) mageInUse[0] = null;
        base.EnemyDeath();

    }

}


    