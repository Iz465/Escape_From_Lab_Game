using System.Collections;
using System.Diagnostics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentMage : Agent
{

    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyAttack;


    private int Health;
    private int PlayerHealth;
    private bool isAttacking;



    public override void OnEpisodeBegin()
    {

        Health = 100;
        PlayerHealth = 100;
        transform.localPosition = new Vector3(0, 0, 0);


       
    }




    private bool isInCover;
    public override void CollectObservations(VectorSensor sensor)
    {
  
        sensor.AddObservation(player.position - transform.position);
        sensor.AddObservation(Vector3.Distance(player.position, transform.position));
        sensor.AddObservation(Health / 100f);
        sensor.AddObservation(PlayerHealth / 100f);


        isInCover = Physics.Raycast(transform.position + Vector3.up * 1f,
                                 (player.position - transform.position).normalized,
                                 out RaycastHit hit,
                                 Vector3.Distance(transform.position, player.position))
                 && hit.transform != player;

        sensor.AddObservation(isInCover ? 1f : 0f);

    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        // getting the two values will be for the x moving and z moving

        float x = actions.ContinuousActions[0];
        float z = actions.ContinuousActions[1];

        float attack = actions.ContinuousActions[2];
        bool isAttack = attack > 0.5f && !isAttacking;


        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


     
        AddReward(-0.001f);




        Vector3 move = new Vector3(x, 0, z) * Time.deltaTime * 3f;
        
        // dont move when attacking
        if (!isAttacking) transform.position += move;

        if (isAttack && !isInCover) Attack();



        if (move.sqrMagnitude > 1e-5f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Power")
        {
            SetReward(-1);
            Health -= 10;

            if (Health <= 0)
                EndEpisode();
        }
        
            
      
    }


    private void Attack()
    {
        isAttacking = true;

        GameObject enemyInstance = Instantiate(enemyAttack, transform.position, transform.rotation);

        if (!enemyInstance) return;
        Rigidbody rb = enemyInstance.GetComponent<Rigidbody>();
        if (!rb) return;
        Collider collider = player.GetComponent<Collider>();
        Vector3 aimDir = (collider.bounds.center - transform.position).normalized;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.AddForce(aimDir * 80, ForceMode.Impulse);


        StartCoroutine(ResetAttack(3));
    }


    public void HitPlayer()
    {
        SetReward(1);
        PlayerHealth -= 10;
        
        if (PlayerHealth <= 0) 
            EndEpisode();
    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);  
        isAttacking = false;
    }

}




// mlagents-learn D:/Unity_Projects/Escape_From_Lab_Game/config/MageAgent.yaml --run-id=MageTest1 --force

// mlagents-learn D:/Unity_Projects/Escape_From_Lab_Game/config/MageAgent.yaml --run-id=MageTest1 --resume

