using UnityEngine;

public class Enemy : MonoBehaviour, IDamageTaken
{
    [SerializeField]
    protected float health;
    [SerializeField]
    public float damage;

    

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        Debug.Log($"Enemy Health : {health}");
        if (health <= 0)
            enemyDeath();

    }

    
    virtual protected void enemyDeath()
    {
        Destroy(gameObject);
    }
    public Transform player;
    public CharacterController controller;
    public float walkSpeed;
    [SerializeField]
    protected float attackRange;

    // Update is called once per frame
   virtual public void Update()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > attackRange)
            controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
        else
            Attack();
        
    }

    virtual protected void Attack()
    {

    }

    
}
