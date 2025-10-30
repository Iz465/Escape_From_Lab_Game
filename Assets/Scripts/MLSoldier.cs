using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using NUnit.Framework;
using System.Collections.Generic;

public class MLSoldier : Agent
{
    [SerializeField] Transform target;
    public float health;
    public float maxHealth;
    public LayerMask raycastLayer;
    float lastDamage;

    public List<Transform> walls = new List<Transform>();

    [SerializeField] CharacterController controller;

    public float walkSpeed = 4;
    public bool showReward;
    public bool showWins;

    public override void OnEpisodeBegin()
    {
        float upperX = 325.3f;
        float lowerX= 307.6f;

        float upperZ = 33.6f;
        float lowerZ = 12.2f;
        //reset agent back to standard, rotation, position, health, whatever
        transform.position = new Vector3(upperX, 0.5f, upperZ);
        //transform.position = new Vector3(Random.Range(lowerX, upperX), 0.5f, Random.Range(lowerZ, upperZ));
        health = maxHealth;

        //randomise target within standard values for smarter agent tracking
        //target.position = new Vector3(Random.Range(lowerX, upperX), 0.5f, Random.Range(lowerZ, upperZ));
        //target.GetComponent<Trash>().health = 100;
        //base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        float first = actions.ContinuousActions[0];
        float second = actions.ContinuousActions[1];
        print("first: "+first.ToString());
        print("second: "+second.ToString());
        transform.position += new Vector3(first, 0, second) * Time.deltaTime * walkSpeed;
        //GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(first, 0, second) * Time.deltaTime * walkSpeed);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            SetReward(1);
            EndEpisode();
            //AddReward(-0.1f);
            //health -= 5;
        }
        if (other.transform.CompareTag("Wall"))
        {
            SetReward(-1);
            EndEpisode();
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-0.1f);
            OnEpisodeBegin();
        }
        /*if (collision.transform.CompareTag("Player"))
        {
            AddReward(.1f);
            EndEpisode();
        }
          //BadMistake();
    }
    
    /*void IsInRange()
    {
        if((transform.position-target.position).magnitude < 20)
        {
            AddReward(20 - (transform.position - target.position).magnitude* 0.005f);
        }
        else
        {
            AddReward(-0.01f);
        }
    }*/

    void LethalMistake()
    {
        SetReward(-3);
        EndEpisode();
    }


    void IsBehindWall()
    {
        Vector3 direction = target.position - transform.position;

        RaycastHit hit;
        bool normalRay = Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, raycastLayer);

        if(normalRay && hit.transform.CompareTag("Player")){
            RaycastHit hitinfo;
            bool widerRay = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), direction, out hitinfo);
            if (widerRay && hitinfo.transform.CompareTag("Obstacle"))
            {
                AddReward(1);
            }
        }
    }

    void Finish()
    {
        if (showWins)
            print("won");
        AddReward(2);
        //SetReward(1);
        EndEpisode();
    }

    private void FixedUpdate()
    {
        //if(showReward)
            //print("reward: "+GetCumulativeReward().ToString());
        //IsBehindWall();
        //IsInRange();

        //transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        /*if (health <= 0)
        {
            LethalMistake();
        }*/

        /*Vector3 direction = target.position - transform.position;
        RaycastHit rayHit;
        if(Physics.Raycast(transform.position, direction.normalized, out rayHit, direction.magnitude, raycastLayer))
        {
            if (rayHit.transform.CompareTag("Player"))
            {
                Trash player = rayHit.transform.GetComponent<Trash>();
                if (player.TakeDamage())
                    AddReward(0.05f);
                

                if(player.health <= 0)
                {
                    Finish();
                }
            }
        }*/
    }
}
