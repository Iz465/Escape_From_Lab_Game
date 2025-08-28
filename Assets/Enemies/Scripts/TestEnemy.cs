using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageTaken
{
    private float health;

    private void Awake()
    {
        health = 100; // make this scriptable object later

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Health : {health}");
        if (health <= 0) 
            Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
    
}
