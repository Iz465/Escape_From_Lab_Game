using UnityEngine;

public class Enemy : MonoBehaviour, IDamageTaken
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    IGetHealth getHealth;
    

    public void takeDamage(int damageTaken, GameObject weapon)
    {
      //  Debug.Log($"Enemy has been hit! {damageTaken} damage has been dealt");
        health -= damageTaken;
        getHealth = weapon.GetComponent<IGetHealth>();
        if (getHealth != null)
            getHealth.GetHealth();
        else
            Debug.Log(weapon);
        if (health <= 0)
            enemyDeath();
      //  Debug.Log($"Enemy has {health} left");
    }


    virtual protected void enemyDeath()
    {
        Destroy(gameObject);
    }
}
