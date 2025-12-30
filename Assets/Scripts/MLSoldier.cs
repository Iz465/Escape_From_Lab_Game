using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class MLSoldier : Agent
{
    [SerializeField] Transform target;
    public float health;
    public float maxHealth;
    public LayerMask raycastLayer;
    float lastDamage;

    public List<Transform> walls = new List<Transform>();
    public List<Transform> goodWalls = new List<Transform>();

    //[SerializeField] CharacterController controller;

    public float walkSpeed = 4;
    bool showReward;
    bool showWins;

    [SerializeField] CharacterController body;

    float upperX, lowerX, upperZ, lowerZ;
    LayerMask mask;
    float wentBehindPlayer;
    Vector3 start, playerStart;

    private void Start()
    {
        upperX = walls[0].transform.position.x;
        lowerX = walls[1].transform.position.x;
        upperZ = walls[0].transform.position.z;
        lowerZ = walls[1].transform.position.z;
        

        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        if(!GameObject.FindGameObjectWithTag("Player"))
            yield return new WaitForEndOfFrame();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        start = transform.position;
        playerStart = target.position;
        yield return null;
    }

    public override void OnEpisodeBegin()
    {
        //transform.position = start;
        //target.position = playerStart;
        target.position = new Vector3(Random.Range(lowerX, upperX), playerStart.y, Random.Range(lowerZ, upperZ));
        transform.position = new Vector3(Random.Range(lowerX, upperX), start.y, Random.Range(lowerZ, upperZ));
        health = maxHealth;
        print("reset");
        //target.position = new Vector3(Random.Range(lowerX, upperX), 0.5f, Random.Range(lowerZ, upperZ));
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

        foreach(Transform wall in goodWalls)
        {
            sensor.AddObservation(wall.lossyScale);
            sensor.AddObservation(wall.rotation);
            sensor.AddObservation(wall.position);
        }

    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        float first = actions.ContinuousActions[0];
        float second = actions.ContinuousActions[1];

        body.Move(new Vector3(first, 0, second) * walkSpeed * Time.deltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");

        print(actions[0].ToString()+" " + actions[1].ToString());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Wall"))
        {
            AddReward(-0.01f);
            EndEpisode();
        }
        if (hit.transform.CompareTag("Obstacle"))
        {
            AddReward(0.01f);
        }
        if (hit.transform.CompareTag("Player"))
        {
            SetReward(-0.1f);
            EndEpisode();
        }
        print(hit.transform.name);
    }

    void IsBehindWall()
    {
        Vector3 direction = target.position - transform.position;

        RaycastHit hit;
        bool normalRay = Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, raycastLayer);

        if (!normalRay) return;
        if (hit.transform.CompareTag("Player"))
        {
            RaycastHit hitinfo;
            bool widerRay = Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), direction, out hitinfo);
            if (widerRay)
            {
                if (hitinfo.transform.CompareTag("Obstacle"))
                {
                    print("found perfect spot");
                    SetReward(2);
                    EndEpisode();
                }
                else
                {
                    if (hitinfo.transform.CompareTag("Player"))
                    {
                        AddReward(-0.01f);
                    }
                }
            }
        }
        else
        {
            AddReward(-0.01f);
        }
    }


    void IsCloseToWall()
    {
        float bestDist = float.MaxValue;
        Transform foundWall = null;

        foreach(Transform wall in goodWalls)
        {
            if ((wall.position - target.position).magnitude < 10)
            {
                if((transform.position - wall.position).magnitude < 10)
                {
                    AddReward(-0.01f);
                }
                continue;
            }

            float wallDist = (wall.position - transform.position).magnitude;
            if (wallDist < 25 && bestDist > wallDist)
            {
                bestDist = wallDist;
                foundWall = wall;
            }
        }

        if(foundWall != null)
        {
            AddReward(Mathf.Clamp01((25f - bestDist) / 25f) * 0.02f);
        }
    }

    private void FixedUpdate()
    {
        //if (!Academy.Instance.IsCommunicatorOn) return;
        float dist = (transform.position - target.position).magnitude;
        if (dist < 15)
        {
            AddReward(-0.01f);
        }
        if(dist < 100)
        {
            AddReward(0.01f);
        }

        //Collision();
        /*if(
            transform.position.x > upperX||
            transform.position.x <  lowerX||
            transform.position.z > upperZ||
            transform.position.z < lowerZ)
        {
            print("outside bounds");
            AddReward(-0.001f);
            EndEpisode();
        }*/

        IsCloseToWall();
        IsBehindWall();

    }
}
