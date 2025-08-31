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
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;

        if (direction.magnitude > 5)
            controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
        else
            Debug.Log("Attack");
    }

    
}
