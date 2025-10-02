using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.AI;

public class navmeshtestscript : MonoBehaviour
{
    // Stats
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamDelay = 5f;
    [SerializeField] protected float attackRange;
    [SerializeField] private GameObject corpse;
    [SerializeField] private string enemyPrefab;

    // Object References
    [Header("Objects")]
    protected Player player;

    // Agent variables
    protected NavMeshAgent agent;
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

        if (distanceToPlayer <= 40)  
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


    virtual protected void ChasePlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0; // keeps horizontal rotation only
      //  transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 0.007f);

        if (distanceToPlayer > attackRange && canAttack)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }

        else if (distanceToPlayer <= attackRange && canAttack)
            AttackPlayer();
            
        
    }

    virtual protected void AttackPlayer()
    {
        agent.isStopped = true;
        animator.SetBool("CanAttack", true);
        canAttack = false;
            
        
         /*   agent.isStopped = true;
            int num = Random.Range(0, 2);
            BloodMage mage = GetComponent<BloodMage>();
            Brute brute = GetComponent<Brute>();
            if (num == 0)
                animator.SetBool("CanAttack", true);
            if (num == 1 && mage)
                animator.SetBool("Beam", true);
            if (num == 1 && brute)
                brute.Charge();

            canAttack = false; 
         */
        
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





