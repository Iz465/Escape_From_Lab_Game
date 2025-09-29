using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class navmeshtestscript : MonoBehaviour
{
    // Stats
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamDelay = 5f;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject corpse;
    [SerializeField] private string enemyPrefab;

    // Object References
    [Header("Objects")]
    protected Player player;

    // Agent variables
    private NavMeshAgent agent;
    protected Animator animator;
    protected CharacterController controller;
    private float timer = 0f;
    public bool canAttack = true;
    protected float distanceToPlayer;
    public static List<GameObject> deadEnemies = new List<GameObject>();

    virtual protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = roamDelay;

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        player = FindAnyObjectByType<Player>();

      
    }
    virtual protected void Update()
    {
        timer += Time.deltaTime;

    
        if (!agent) return;
        if (!player) return;
        if (!animator) return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        animator.SetFloat("PlayerDistance", distanceToPlayer);

        if (distanceToPlayer <= 40)
        {
            
            Vector3 lookDir = player.transform.position - transform.position;
            lookDir.y = 0; // keep only horizontal rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);

            if (distanceToPlayer > 10 && canAttack)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
              
            if (distanceToPlayer <= attackRange && canAttack)
            {
                agent.isStopped = true;
                animator.SetBool("CanAttack", true);
                canAttack = false;
            }
            
        }


        else if (timer > roamDelay)
        {
            Vector3 newPos = RandomLocation();
            agent.SetDestination(newPos);
            timer = 0;
        }


        if (agent.velocity.magnitude == 0)
            animator.SetBool("Roam", false);
        else
            animator.SetBool("Roam", true);
    }



    private Vector3 RandomLocation()
    {
        float randomDir = Random.Range(5f, roamRadius);
        Vector3 direction = Random.insideUnitSphere * randomDir;
        direction += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, roamRadius, -1);

        return hit.position;
    }


    virtual protected void Attack()
    {
        Debug.Log("Attack!");
    }

    public void TakeDamage(float damageTaken)
    {
        Debug.Log("Taking damage!");
        health -= damageTaken;
        if (health <= 0)
            EnemyDeath();

    }

    virtual protected void EnemyDeath()
    {
        player.stats.health += 10;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, 100);
        if (corpse)
        {
      
            GameObject ragdoll = Instantiate(corpse, transform.position, Quaternion.identity);
            Vector3 hitDirection = (ragdoll.transform.position - player.transform.position).normalized;
            Rigidbody[] rb = ragdoll.GetComponentsInChildren<Rigidbody>();


    

            foreach (Rigidbody rigid in rb)
            {
                rigid.AddForce(hitDirection * 15, ForceMode.VelocityChange);
            }
             
        

            GameObject prefab = Resources.Load<GameObject>(enemyPrefab);
            if (prefab)
                deadEnemies.Add(prefab);

            else
                Debug.LogWarning($"Prefab : {enemyPrefab} Not available");
        }

        Destroy(gameObject);
    }



}





