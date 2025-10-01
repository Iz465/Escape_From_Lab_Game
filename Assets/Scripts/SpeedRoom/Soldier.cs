using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : ChaseAI
{
    public Transform plr;
    PlayerInfo playerInfo;

    [SerializeField] float shootDelay, shootSpeed, timeBetweenBullet;

    float timeWhenSeenPlayer = 0;
    float lastBullet;

    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        while (!GameObject.FindGameObjectWithTag("Player"))
        {
            yield return null;
        }
        if (GameObject.Find("Camera"))
        {
            Destroy(GameObject.Find("Camera"));
        }

        plr = GameObject.FindGameObjectWithTag("Player").transform;
        playerInfo = plr.GetComponent<PlayerInfo>();

        //Chase(plr);
    }

    private void OnEnable()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForPlayer());
    }
    // Update is called once per frame
    void Update()
    {
        if (DetectPlayer())
        {
            ShootPlayer();
        }
    }

    bool DetectPlayer()
    {
        if (DetectObj(plr))
        {
            if (agent.enabled)
            {
                timeWhenSeenPlayer = Time.time;
                agent.enabled = false;
            }
            return true;
        }
        else
        {
            agent.enabled = true;
        }
        return false;
    }
    
    void ShootPlayer()
    {
        Vector3 direction = (plr.position - transform.position).normalized;
        Vector3 face = transform.forward;

        float angleInRadians = Vector3.Dot(face, direction);
        float degrees = angleInRadians * 180 / Mathf.PI;
        float turn = Mathf.Acos(angleInRadians);

        print(turn);
        while (angleInRadians != 3)
        {
            direction = (plr.position - transform.position).normalized;
            face = transform.forward;
            transform.Rotate(new Vector3(0,turn, 0));
            angleInRadians = Vector3.Dot(face, direction);
        }



        if (Time.time > timeWhenSeenPlayer + shootDelay)
        {
            timeWhenSeenPlayer += timeBetweenBullet;
            print("shoot");
        }
    }
}
