using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.AI;
using Unity.MLAgents.Actuators;

public class navmeshtestscript : MonoBehaviour // Readd this to to the chase ai script.
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
    [SerializeField] private List<AudioClip> fleshSounds;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] protected List<AudioClip> attackSounds;
    [SerializeField] protected AudioClip attackSound;
   
    protected AudioSource audioSource;

    [System.Serializable]
    public struct CorpseParts
    {
        public GameObject head;
        public GameObject torso;
        public GameObject leftHand;
        public GameObject rightHand;
        public GameObject legs;

    }

    [System.Serializable]
    public struct CorpseLocations
    {
        public Transform head;
        public Transform torso;
        public Transform leftHand;
        public Transform rightHand;
        public Transform legs;

    }


    [SerializeField] private CorpseParts corpseParts;
    [SerializeField] private CorpseLocations corpseLocations;



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
        audioSource = GetComponent<AudioSource>();

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
      
        FacePlayer();

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

    protected void FacePlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0; // keeps horizontal rotation only
        if (canRotate)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotateSpeed);
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
        
        if (fleshSounds.Count > 0)
        {
            int randomSound = Random.Range(0, fleshSounds.Count);
            globalEnemyManager.CheckEnemySound(fleshSounds[randomSound], "flesh", audioSource);
        }
       

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
     
        if (fleshSounds.Count > 0) globalEnemyManager.CheckEnemySound(fleshSounds[0], "flesh", player.audioSource);
        if (deathSound) globalEnemyManager.CheckEnemySound(deathSound, "death", player.audioSource);


        canAttack = true;

        GlobalEnemyManager.enemiesInRange.Remove(gameObject);
        globalEnemyManager.EmptyEnemies(gameObject);

     
        player.stats.health += healthGain;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, player.stats.maxHealth);

        for (int i = 0; i < 4;  i++)
        {
            if (corpseParts.head)
                MakeRagdoll(corpseParts.head, corpseLocations.head, 0);
        }

        if (corpseParts.legs)
            MakeRagdoll(corpseParts.legs, corpseLocations.legs, 0.05f);
        if (corpseParts.rightHand)
            MakeRagdoll(corpseParts.rightHand, corpseLocations.rightHand, 0.05f);
        if (corpseParts.leftHand)
            MakeRagdoll(corpseParts.leftHand, corpseLocations.leftHand, -0.05f);
        if (corpseParts.torso)
            MakeRagdoll(corpseParts.torso, corpseLocations.torso, 0);

        Destroy(gameObject);
    }




    // dismemberment for when enemy dies
    private void MakeRagdoll(GameObject bodypart, Transform spawnLocation, float xValue)
    {
        if (bodypart)
        {
           

            GameObject ragdoll = Instantiate(bodypart, spawnLocation.transform.position, Quaternion.identity);
            Vector3 hitDirection = (ragdoll.transform.position - player.transform.position).normalized;
            ragdoll.transform.rotation = Quaternion.LookRotation(hitDirection) * Quaternion.Euler(90, 0, 0);


            Rigidbody rigid = ragdoll.GetComponent<Rigidbody>();
            if (rigid)
            {
          
              //  hitDirection.y = 0.1f;
                rigid.AddForce(hitDirection.normalized * 15, ForceMode.Impulse);
                rigid.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);
                
            }

            Destroy(ragdoll, 10);
               
        }
    }




}





