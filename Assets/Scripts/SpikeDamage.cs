using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void DamageEnemy(Enemy enemy)
    {
        enemy.takeDamage(20f);
    }

    void DamagePlayer(PlayerData plr)
    {
        plr.health -= 20;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Enemy>() != null)
        {
            DamageEnemy(other.transform.GetComponent<Enemy>());
        }

        if(other.transform.GetComponent<PlayerData>() != null)
        {
            DamagePlayer(other.transform.GetComponent<PlayerData>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Enemy>() != null)
        {
            DamageEnemy(collision.transform.GetComponent<Enemy>());
        }

        if (collision.transform.GetComponent<PlayerData>() != null)
        {
            DamagePlayer(collision.transform.GetComponent<PlayerData>());
        }
    }

    void Update()
    {
        
    }
}
