using UnityEngine;

public class Enemy : MonoBehaviour//, IDamageTaken
{
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float damage;
    IGetHealth getHealth;
    

    public void takeDamage(float damageTaken)
    {

        health -= damageTaken;
      //  getHealth = weapon.GetComponent<IGetHealth>();
     //   if (getHealth != null)
     //       getHealth.GetHealth();
     //   else
       //     Debug.Log(weapon);
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

        controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
    }
}
