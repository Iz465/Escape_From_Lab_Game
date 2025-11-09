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

    Rigidbody body;

    public float upperX, lowerX, upperZ, lowerZ;
    public LayerMask mask;
    float wentBehindPlayer;

    private void Start()
    {
        target = transform.parent.Find("Player");
        upperX = walls[0].transform.position.x;
        lowerX = walls[1].transform.position.x;
        upperZ = walls[0].transform.position.z;
        lowerZ = walls[1].transform.position.z;
        body = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        /*float upperX = 325.3f;
        float lowerX= 307.6f;

        float upperZ = 33.6f;
        float lowerZ = 12.2f;*/
        //reset agent back to standard, rotation, position, health, whatever
        //transform.position = new Vector3(lowerX, 0.5f, lowerZ);
        transform.position = new Vector3(Random.Range(lowerX, upperX), 0.5f, Random.Range(lowerZ, upperZ));
        health = maxHealth;

        //randomise target within standard values for smarter agent tracking
        target.position = new Vector3(Random.Range(lowerX, upperX), 0.5f, Random.Range(lowerZ, upperZ));
        //target.GetComponent<Trash>().health = 100;
        //base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);
        sensor.AddObservation(target.forward);

        sensor.AddObservation(upperX);// = walls[0].transform.position.x;
        sensor.AddObservation(lowerX);// = walls[1].transform.position.x;
        sensor.AddObservation(upperZ);// = walls[0].transform.position.z;
        sensor.AddObservation(lowerZ);// = walls[1].transform.position.z;

        Vector3 dir = (target.position - transform.position);
        sensor.AddObservation(dir.normalized);
        sensor.AddObservation(dir.magnitude);

        

    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        float first = actions.ContinuousActions[0];
        float second = actions.ContinuousActions[1];

        //transform.position += new Vector3(first, 0, second) * Time.deltaTime * walkSpeed;
        //GetComponent<Rigidbody>().Move(new Vector3(first, 0, second) * Time.deltaTime * walkSpeed, Quaternion.identity);
        //GetComponent<Rigidbody>().position += new Vector3(first, 0, second) * Time.deltaTime * walkSpeed;
        body.MovePosition(transform.position + new Vector3(first, 0, second)*Time.deltaTime*walkSpeed);
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

    /*void Collision()
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
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Player hit");
            //SetReward(1);
            //EndEpisode();

            //AddReward(-0.1f);
            //health -= 5;
        }
        if (other.transform.CompareTag("Wall"))
        {
            //SetReward(-1);
            //EndEpisode();
        }
        print(other.transform.tag);
    }*/
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            print("hit player");
            AddReward(-0.01f);
            //EndEpisode();
            //AddReward(-0.1f);
            //health -= 5;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            print("hit player");
            AddReward(-0.01f);
            //EndEpisode();
            //AddReward(-0.1f);
            //health -= 5;
        }
        /*if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-0.5f);
            OnEpisodeBegin(); // reset position without ending abruptly
        }*/
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

    void IsBehindPlayer()
    {
        Vector3 playerDir = target.position - transform.position;
        Vector3 playerFace = target.forward;

        if(playerDir.magnitude < 4 && playerDir.magnitude > 2)
        {
            if (Vector3.Dot(playerDir.normalized, playerFace) < 0.5f)
            {
                float angleResult = -Vector3.Dot(playerDir.normalized, playerFace);
                wentBehindPlayer += Time.deltaTime;
                AddReward(angleResult * 0.02f);

                if (wentBehindPlayer > 5)
                    Finish();
            }
            else
            {
                AddReward(-0.01f);
                wentBehindPlayer = 0;
            }
        }
        
    }

    private void FixedUpdate()
    {
        //Collision();

        if(
            transform.position.x > upperX||
            transform.position.x <  lowerX||
            transform.position.z > upperZ||
            transform.position.z < lowerZ)
        {
            AddReward(-0.001f);
            OnEpisodeBegin();
        }

        float dist = ((transform.position - target.position).magnitude);
        if(dist > 2)
            AddReward(Mathf.Clamp01(1f - dist / 10f) * 0.002f);

        IsBehindPlayer();

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
