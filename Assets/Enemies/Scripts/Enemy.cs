using UnityEngine;

public class Enemy : MonoBehaviour, IDamageTaken
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    

    public void takeDamage(int damageTaken)
    {
        Debug.Log($"Enemy has been hit! {damageTaken} damage has been dealt");
        health -= damageTaken;
        if (health <= 0)
            enemyDeath();
        Debug.Log($"Enemy has {health} left");
    }


    virtual protected void enemyDeath()
    {
        Destroy(gameObject);
    }
}
