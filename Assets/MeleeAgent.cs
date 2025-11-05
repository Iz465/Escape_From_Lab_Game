using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.AI;
using Unity.Transforms;
using System.Linq;
using System;

public class MeleeAgent : Agent
{
    [SerializeField] private Transform player;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    [SerializeField] private Transform planeTransform;
    [SerializeField] ExplosiveBlood explosiveBlood;

    private float previousDistance;


    // obserrve -> decision -> action
    // continuous = float
    // discrete = int
    // space vector = how many inputs im giving the ai. (aka the  inputs in observation function)


    public override void OnEpisodeBegin()
    {

        bool playerNotOnMine = false;
        transform.localPosition = new Vector3(10.38f, 0, -7.98f);

        while (!playerNotOnMine)
        {
         
            player.localPosition = new Vector3(UnityEngine.Random.Range(-15, 37), 0.64f, UnityEngine.Random.Range(-26, 25));
            previousDistance = Vector3.Distance(transform.position, player.position);
            playerNotOnMine = true;
            foreach (ExplosiveBlood trap in FindObjectsOfType<ExplosiveBlood>())
            {
                while (Vector3.Distance(trap.transform.position, transform.position) < 5)
                    trap.transform.localPosition = new Vector3(UnityEngine.Random.Range(-20, 37), 0.64f, UnityEngine.Random.Range(-26, 25));
                if (Vector3.Distance(player.transform.position, trap.transform.position) < 2f)
                {
                    playerNotOnMine = false;
                    break;
                }

            }

        }
 

    

    }



    // giving the ai the environment info it needs
    // space vector will be 6 as im passing in 6 values. 3 values for the players position (x y and z) and 3 for the agent (x y and z);

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("OBSERVATION");
        sensor.AddObservation(player.position - transform.position);





    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        // getting the two values will be for the x moving and z moving
        float x = actions.ContinuousActions[0];
        float z = actions.ContinuousActions[1];

 

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        AddReward(previousDistance - distanceToPlayer);

        // Constant negative reward that is small to encourage the agent to not stand still and keep looking for player
        AddReward(-0.001f);

        previousDistance = distanceToPlayer;


        Vector3 move = new Vector3(x, 0, z) * Time.deltaTime * 3f;
        transform.position += move;


        Quaternion targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);





    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collided with: {collision}");
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Trigger enter of: {other}");
        ExplosiveBlood explosiveBlood = other.GetComponent<ExplosiveBlood>();
        if (explosiveBlood)
        { 
            floorMeshRenderer.material = loseMaterial;
            AddReward(-1f);
        }
      

        if (other.CompareTag("Player"))
        {
            floorMeshRenderer.material = winMaterial;
            AddReward(1f);
        }
       

        if (other.CompareTag("Wall"))
        {
            floorMeshRenderer.material = loseMaterial;
            AddReward(-1f);
        }
      


        EndEpisode();
    }

}
// mlagents-learn D:/Unity_Projects/Escape_From_Lab_Game/config/MeleeAgent.yaml --run-id=MeleeEnemyTest1 --force

// mlagents-learn D:/Unity_Projects/Escape_From_Lab_Game/config/MeleeAgent.yaml --run-id=MeleeEnemyTest1 --resume

// pip install torch==2.2.0



