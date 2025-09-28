using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public bool damaged;

    void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(20f);
    }

    void DamagePlayer(PlayerInfo plr)
    {
        if (damaged) return;
        damaged = true;
        plr.health -= 20;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Enemy>() != null)
        {
            DamageEnemy(other.transform.GetComponent<Enemy>());
        }

        if(other.transform.GetComponent<PlayerInfo>() != null)
        {
            DamagePlayer(other.transform.GetComponent<PlayerInfo>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Enemy>() != null)
        {
            DamageEnemy(collision.transform.GetComponent<Enemy>());
        }

        if (collision.transform.GetComponent<PlayerInfo>() != null)
        {
            DamagePlayer(collision.transform.GetComponent<PlayerInfo>());
        }
    }
}
