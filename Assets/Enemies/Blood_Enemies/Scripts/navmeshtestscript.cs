using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.AI;

public class navmeshtestscript : MonoBehaviour // Readd this to to the chase ai script.
{
    // Stats
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamDelay = 5f;
    [SerializeField] protected float attackRange;
    [SerializeField] private GameObject corpse;

    [Header("Blood Stuff")]
    [SerializeField] protected GameObject blood;
    [SerializeField] protected List<Transform> bloodHits;


    [SerializeField] private string enemyPrefab;
    private static GameObject currentEnemyAttacking;

    [System.Serializable] public struct CorpseParts
    {
        public GameObject head;
        public GameObject torso;
        public GameObject leftHand;
        public GameObject rightHand;
        public GameObject legs;

    }

    // Object References
    [Header("Objects")]
    protected Player player;

    // Agent variables
    protected NavMeshAgent agent;
    protected Animator animator;
    protected CharacterController controller;
    private Collider enemyCollider;
    private float timer = 0f;
    public bool canAttack = true;
    protected float distanceToPlayer;
    public static List<GameObject> deadEnemies = new List<GameObject>();
    protected GlobalEnemyManager globalEnemyManager;
    private Brute brute;
    protected bool canRotate;


    protected float rotateSpeed;

    virtual protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = roamDelay;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = FindAnyObjectByType<Player>();
        rotateSpeed = 5f;
        globalEnemyManager = FindFirstObjectByType<GlobalEnemyManager>();
        GlobalEnemyManager.levelComplete = false;
        if (globalEnemyManager)
            globalEnemyManager.AddEnemy(gameObject);
        brute = GetComponent<Brute>(); // bandaid
        canRotate = true;
        enemyCollider = GetComponent<Collider>();
    }

    virtual protected void Update()
    {
        timer += Time.deltaTime;

        if (!agent) return;
        if (!player) return;
        if (!animator) return;

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

    private static int random;
    
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

            if (agent.isOnNavMesh && !brute) // bandaid
                agent.isStopped = true;

            if (canAttack)
            {
                AttackPlayer();
             
            /*    random = globalEnemyManager.RandomiseAttack();
                int num = 0;
                foreach (GameObject enemy in GlobalEnemyManager.enemiesInRange)
                {
                    if (num == random)
                        if (gameObject == enemy)
                            if (enemy != currentEnemyAttacking && GlobalEnemyManager.enemiesInRange.Count > 1)
                            {
                                currentEnemyAttacking = enemy;
                                AttackPlayer();
                            }
                          
                            else if (GlobalEnemyManager.enemiesInRange.Count == 1)
                                AttackPlayer();
                    num++; 
                } */
            
            }
        
        }
            
        
    }


    virtual protected void AttackPlayer()
    {
        animator.SetBool("CanAttack", true);
       
        canAttack = false;
           
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

    virtual public void TakeDamage(float damageTaken)
    {
        Debug.Log(enemyCollider);
        if (player.playerHitParticle) Instantiate(player.playerHitParticle, enemyCollider.bounds.center + new Vector3(0, 2, 0), transform.rotation); // bandaid solution
        if (blood) ShowBlood();
        health -= damageTaken;
        Debug.Log($"Taking damage! Health Left : {health}");
        if (health <= 0)
            EnemyDeath();

    }

    private void ShowBlood()
    {
        
        foreach (Transform bloodLocation in bloodHits)
        {
           GameObject bloodInstance = Instantiate(blood, bloodLocation);
           Destroy(bloodInstance, 0.5f); 
        }
    }

    [SerializeField] private CorpseParts corpseParts = new CorpseParts();
    [SerializeField] private float healthGain;
    virtual protected void EnemyDeath()
    {

        canAttack = true;

        GlobalEnemyManager.enemiesInRange.Remove(gameObject);
        globalEnemyManager.EmptyEnemies(gameObject);

     
        player.stats.health += healthGain;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, 100);
        
 

   /*     if (corpse)
        {
            GameObject prefab = Resources.Load<GameObject>(enemyPrefab);
            if (prefab)
                deadEnemies.Add(prefab);

            else
                Debug.LogWarning($"Prefab : {enemyPrefab} Not available");
        }
   */
        
        if (corpseParts.head)
            MakeRagdoll(corpseParts.head,3);
        if (corpseParts.legs)
            MakeRagdoll(corpseParts.legs,1);
        if (corpseParts.rightHand)
            MakeRagdoll(corpseParts.rightHand, 2.5f);
        if (corpseParts.leftHand)
            MakeRagdoll(corpseParts.leftHand,2.5f);
        if (corpseParts.torso)
            MakeRagdoll(corpseParts.torso, 2.5f);

        Destroy(gameObject);
    }

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



}





