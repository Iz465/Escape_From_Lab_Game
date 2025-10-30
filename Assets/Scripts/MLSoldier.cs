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

    public float upperX, lowerX, upperZ, lowerZ;
    public LayerMask mask;
    public override void OnEpisodeBegin()
    {
        /*float upperX = 325.3f;
        float lowerX= 307.6f;

        float upperZ = 33.6f;
        float lowerZ = 12.2f;*/
        //reset agent back to standard, rotation, position, health, whatever
        transform.position = new Vector3(lowerX, 0.5f, lowerZ);
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

        Vector3 dir = (target.position - transform.position);
        sensor.AddObservation(dir.normalized);
        sensor.AddObservation(dir.magnitude);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        float first = actions.ContinuousActions[0];
        float second = actions.ContinuousActions[1];

        //transform.position += new Vector3(first, 0, second) * Time.deltaTime * walkSpeed;
        GetComponent<Rigidbody>().position += new Vector3(first, 0, second) * Time.deltaTime * walkSpeed;
        //GetComponent<Rigidbody>().MovePosition(transform.position + );
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");

    }

    void CheckCollision(RaycastHit hit)
    {
        print(hit.transform.name);
        if (hit.transform.CompareTag("Player"))
        {
            AddReward(1);
            EndEpisode();
        }
        if (hit.transform.CompareTag("Wall"))
        {
            AddReward(-1);
            EndEpisode();
        }
    }

    void Collision()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 1, mask))
        {
            CheckCollision(hit);
        }
        if(Physics.Raycast(transform.position, Vector3.right, out hit, 1, mask))
        {
            CheckCollision(hit);
        }
        if(Physics.Raycast(transform.position, -Vector3.right, out hit, 1, mask))
        {
            CheckCollision(hit);
        }
        if(Physics.Raycast(transform.position, -Vector3.forward, out hit, 1, mask))
        {
            CheckCollision(hit);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Player hit");
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
        print(other.transform.tag);
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            print("hit player");
            SetReward(1);
            EndEpisode();
            //AddReward(-0.1f);
            //health -= 5;
        }
        if (collision.transform.CompareTag("Wall"))
        {
            SetReward(-1);
            EndEpisode();
        }
        print(collision.transform.tag);
        /*
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
          //BadMistake();*/
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
        //Collision();
        if(transform.position.x > upperX+5)
        {
            AddReward(-1);
            EndEpisode();
        }
        if(transform.position.x < lowerX-5)
        {
            AddReward(-1);
            EndEpisode();
        }

        if(transform.position.z > upperZ+5)
        {
            AddReward(-1);
            EndEpisode();
        }

        if (transform.position.z < lowerZ-5)
        {
            AddReward(-1);
            EndEpisode();
        }

        if((transform.position - target.position).magnitude < 3)
        {
            print("player hit");
            AddReward(1);
            EndEpisode();
        }
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
