using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    

    public void TakeDamage(int damageTaken)
    {
        Debug.Log($"Enemy has been hit! {damageTaken} damage has been dealt");
        health -= damageTaken;
        if (health <= 0)
            enemyDeath();
        Debug.Log($"Enemy has {health} left");
    }
    void OnCollisionEnter(Collision collision)
    {
        print("Collision detected with: " + collision.gameObject.name);
        Transform otherTransform = collision.transform;
        if (otherTransform.name.Contains("spike"))
        {
            damage -= 10; // Assuming spikes deal 10 damage
        }
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

        controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
    }
}
