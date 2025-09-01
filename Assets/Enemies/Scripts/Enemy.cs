using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float damage;

    

    public void TakeDamage(float damageTaken)
    {

        health -= damageTaken;
        if (health <= 0)
            enemyDeath();

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

        controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
    }
}
