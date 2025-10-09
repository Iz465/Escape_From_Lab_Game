using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : ChaseAI
{
    public Transform plr;
    public Transform muzzle;
    public GameObject bullet;
    PlayerInfo playerInfo;

    [SerializeField] float shootDelay, shootSpeed, timeBetweenBullet;
    [SerializeField] float health;
    [SerializeField] float stunned, hitStunTime;

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

        yield return new WaitForSeconds(1);
        plr = GameObject.FindGameObjectWithTag("Player").transform;
        playerInfo = plr.GetComponent<PlayerInfo>();
        print("starting to chase player");
        Chase(plr);
    }

    private void OnEnable()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForPlayer());
    }
    // Update is called once per frame
    void Update()
    {
        if (plr == null) return;
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
            if (!agent.enabled)
            {
                agent.enabled = true;
                Chase(plr);
            }
        }
        return false;
    }
    
    void ShootPlayer()
    {
        Vector3 relativePos = plr.position - transform.position;
        Vector3 normal = relativePos.normalized;

        float forwardAccurate = Vector3.Dot(transform.forward, normal);
        float rightSideAccurate = Vector3.Dot(transform.right, normal);
        float leftSideAccurate = Vector3.Dot(-transform.right, normal);

        //if (forwardAccurate > 0.9f)
          //  transform.LookAt(plr);
        //else
        //{
            if(rightSideAccurate < 0)
            {
                if(plr.GetComponent<Speed>().highSpeedMode)
                    transform.Rotate(new Vector3(0, -Time.deltaTime, 0));
                else
                    transform.Rotate(new Vector3(0, -90*Time.deltaTime, 0));
            }
            else
            {
                if(plr.GetComponent<Speed>().highSpeedMode)
                    transform.Rotate(new Vector3(0, Time.deltaTime, 0));
                else
                    transform.Rotate(new Vector3(0,90*Time.deltaTime, 0));
            }
        //}





        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //float fov = Vector3.Dot(transform.forward, relativePos.normalized);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, fov+Time.deltaTime);

        if (Time.time > timeWhenSeenPlayer + shootDelay && Time.time > stunned)
        {
            timeWhenSeenPlayer += timeBetweenBullet;
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.parent = null;
            newBullet.tag = "Bullet";
            newBullet.transform.position = muzzle.position;
            newBullet.transform.LookAt(plr.position);
            newBullet.GetComponent<Rigidbody>().AddForceAtPosition(newBullet.transform.forward * 10000, newBullet.transform.position);
            //newBullet.GetComponent<Rigidbody>().linearVelocity = newBullet.transform.forward * 1000;
            print("shoot");
        }
    }

    void GotHit()
    {
        health -= 13;
        stunned = Time.time + hitStunTime;
        if(health < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
            GotHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
            GotHit();
    }
}
