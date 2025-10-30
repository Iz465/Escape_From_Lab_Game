using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using static UnityEngine.UI.Image;

public class AgentMage : navmeshtestscript
{
    [SerializeField] private GameObject power;
    [SerializeField] private float speed;
    [SerializeField] private float instantAttackSpeed;
    [SerializeField] private Transform aimLoc;
    [SerializeField] private GameObject circleInstantPrefab;
    [SerializeField] private GameObject instantAttackPrefab;
    [SerializeField] private LayerMask playerLayer;



    protected override void ChasePlayer()
    {
        FacePlayer();
        agent.SetDestination(player.transform.position);

        if (distanceToPlayer > attackRange)
        {
            agent.isStopped = false;
            canAttack = false;
        }
        
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            canAttack = true;
        }
           
    }

    protected override void AttackPlayer()
    {

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
        rb.AddForce(aimDir * speed, ForceMode.Impulse); 
         
    }



    private IEnumerator ResetAnim(int time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("CanAttack", false);
        StartCoroutine(ResetAttack(1));

    }


    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds (time);
        
        canAttack = true;
  
    }





    private Vector3 startingPosition;
    private GameObject circleAttackInstance;
    
    private void InstantAttack()
    {
        animator.SetBool("InstantAttack", true);
     
        startingPosition = player.transform.position;
        StartCoroutine(HitPlayer(instantAttackSpeed));
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


    // Agent functions

    // this function lets the agent collect information it needs for training. 
   
    public override void CollectObservations(VectorSensor sensor)
    {
        // Shows the distance it is from the player
        Vector3 distanceFromPlayer = player.transform.position - transform.position;


        // sensor.AddObservation is required to TELL the agent what you want. In this case, the distance to the player.
        sensor.AddObservation(distanceFromPlayer);
     

        // Telling the agent the distance from the player in a float value
        sensor.AddObservation(distanceFromPlayer.magnitude);


        // Tells agent whether enemy can attack or not
        // Any number that is above 1 means true
        // If number is 0 it means false.
        sensor.AddObservation(canAttack ? 1 : 0); // Tells agent 1 if true and 0 if false
 
    }


    // Whenever the agent does an action to decide when and which attack should be used.
    public override void OnActionReceived(ActionBuffers actions)
    {
        int chosenAttack = actions.DiscreteActions[0]; 
        //    int attackChoice = actions.DiscreteActions[0];
    }



}




















/*
   if (attack)
        animator.SetBool("CanAttack", true);

    else if (instantAttack)
        animator.SetBool("InstantAttack", true);
*/