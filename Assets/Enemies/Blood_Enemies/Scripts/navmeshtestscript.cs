using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.AI;
using Unity.MLAgents.Actuators;

public class navmeshtestscript : Agent // Readd this to to the chase ai script.
{
    // Stats
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamDelay = 5f;
    [SerializeField] protected float attackRange;
    [SerializeField] private string attackName;
   
    [SerializeField] public bool canHitMultiple = false;

    [Header("Blood Stuff")]
    [SerializeField] protected GameObject blood;
    [SerializeField] protected List<Transform> bloodHitLocations;
    [SerializeField] private Transform particleHitLocation;
    [SerializeField] private List<GameObject> CorpseParts;
  

   
    [Header("Objects")]
    protected Player player;

    // Agent variables
    protected NavMeshAgent agent;
    protected Animator animator;
    private float timer = 0f;
    [HideInInspector] public bool canAttack = true;
    protected float distanceToPlayer;

    protected GlobalEnemyManager globalEnemyManager;
    
    protected bool canRotate = true;
    [SerializeField] protected float rotateSpeed = 5f;

    virtual protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = roamDelay;
        animator = GetComponent<Animator>();
        player = FindAnyObjectByType<Player>();
        globalEnemyManager = FindFirstObjectByType<GlobalEnemyManager>();
        if (globalEnemyManager) globalEnemyManager.AddEnemy(gameObject);

    }

    // Enemy constantly roaming / chasing player depending on options.
    virtual protected void Update()
    {
        timer += Time.deltaTime;

        if (!agent || !player || !animator) return;


        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= roamRadius)  
            ChasePlayer();
            
        
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

  
    // Enemy Chases player and attacks when in certain range
    virtual protected void ChasePlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0; // keeps horizontal rotation only
        if (canRotate)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotateSpeed);


        if (distanceToPlayer > attackRange && canAttack)
        {
            if (!agent.isOnNavMesh) return;
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            GlobalEnemyManager.enemiesInRange.Remove(gameObject);
        }

        else if (distanceToPlayer <= attackRange)
        {
            
            GlobalEnemyManager.enemiesInRange.Add(gameObject);

            if (agent.isOnNavMesh) 
                agent.isStopped = true;

            if (canAttack)
                AttackPlayer();

        }
            
        
    }


    virtual protected void AttackPlayer()
    {
       canAttack = false;
       animator.SetTrigger(attackName);
           
    }


    // When Enemy cant find player it roams around.
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
        // Override in children classes
    }

    // Everytime the enemy gets hit by the player
    virtual public void TakeDamage(float damageTaken)
    {
    
        if (player.playerHitParticle) Instantiate(player.playerHitParticle, particleHitLocation.position, Quaternion.identity); 
        if (blood) ShowBlood();
        health -= damageTaken;
        Debug.Log($"Taking damage! Health Left : {health}");
        if (health <= 0)
            EnemyDeath();
    }

    // Blood particles spawned whenever enemy is hit
    private void ShowBlood()
    {
        
        foreach (Transform bloodLocation in bloodHitLocations)
        {
           GameObject bloodInstance = Instantiate(blood, bloodLocation);
           Destroy(bloodInstance, 0.5f); 
        }
    }


    [SerializeField] private float healthGain;

    virtual protected void EnemyDeath()
    {

        canAttack = true;

        GlobalEnemyManager.enemiesInRange.Remove(gameObject);
        globalEnemyManager.EmptyEnemies(gameObject);

     
        player.stats.health += healthGain;
        // player.stats.health = Mathf.Clamp(player.stats.health, 0, player.stats.maxHealth);


        if (CorpseParts.Count > 0)
            foreach (GameObject corpse in CorpseParts)
                MakeRagdoll(corpse, 2);

        Destroy(gameObject);
    }

    // dismemberment for when enemy dies
    private void MakeRagdoll(GameObject bodypart, float height)
    {
        if (bodypart)
        {

            GameObject ragdoll = Instantiate(bodypart, transform.position + new Vector3(0, height, 0), Quaternion.identity);
            Vector3 hitDirection = (ragdoll.transform.position - player.transform.position).normalized;
            ragdoll.transform.rotation = Quaternion.LookRotation(hitDirection) * Quaternion.Euler(90, 0, 0);


            Rigidbody rigid = ragdoll.GetComponent<Rigidbody>();
            if (rigid)
            {
                
                rigid.AddForce(hitDirection * 20, ForceMode.Impulse);
                rigid.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);
                
            }

            Destroy(ragdoll, 10);
               
        }
    }

    // Reinforcement Learning function
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

}





