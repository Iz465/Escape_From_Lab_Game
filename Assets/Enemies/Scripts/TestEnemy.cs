using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageTaken
{
    public float health;

    private void Awake()
    {
        health = 100; // make this scriptable object later

    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy Health: {health}");
        health -= damage;
        if (health <= 0) 
            Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
    
}
