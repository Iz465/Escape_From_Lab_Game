using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float health;
    [SerializeField]
    public float damage;
    [SerializeField]
    private GameObject corpse;

    public static List<GameObject> deadEnemies = new List<GameObject>();

    [SerializeField]
    private string enemyPrefab;



    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
      

    }

    public void TakeDamage(float damageTaken)
    {

        health -= damageTaken;
        if (health <= 0)
            EnemyDeath();

    }
    /*void OnCollisionEnter(Collision collision)
    {
        print("Collision detected with: " + collision.gameObject.name);
        Transform otherTransform = collision.transform;
        if (otherTransform.name.Contains("spike"))
        {
            damage -= 10; // Assuming spikes deal 10 damage
        }
    }*/

    virtual protected void EnemyDeath()
    {
    
        if (corpse)
        {
            Instantiate(corpse, transform.position, Quaternion.identity);
            
            GameObject prefab = Resources.Load<GameObject>(enemyPrefab);
            if (prefab)
                deadEnemies.Add(prefab);
           
            else
                Debug.LogWarning($"Prefab : {enemyPrefab} Not available");
        }
      
        Destroy(gameObject);
    }



    public Transform player;
    public CharacterController controller;
    public float walkSpeed;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float cooldown;
    protected bool canAttack = true;
    // Update is called once per frame
    protected Vector3 direction;
    [SerializeField]

    void Update()
    {
        direction = player.position - transform.position;

  

        if (direction.magnitude > attackRange)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            if (controller.enabled == true)
                controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
      
        }

        else if (canAttack)
        {
           
            Attack();
            StartCoroutine(ResetAttack(cooldown));
            canAttack = false;
        }
     

    }

    // Implement Unique Attacks for child enemies
    virtual protected void Attack()
    {
   
    }


    protected IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    

}
