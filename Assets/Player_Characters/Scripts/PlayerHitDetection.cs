using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private Player player;
    private bool canDamage = true;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
   
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
            player.TakeDamage(damage);
        Destroy(gameObject);
    }

    
    private void OnTriggerStay(Collider other)
    {
   
        Player player = other.gameObject.GetComponent<Player>();
        if (player && canDamage)
        {
            canDamage = false;
            Invoke(nameof(DamageCooldown), 0.1f);
        }
        

        

    }

    private void OnTriggerEnter(Collider other)
    {

        Player player = other.gameObject.GetComponent<Player>();
        if (player && canDamage)
        {
            canDamage = false;
            Invoke(nameof(DamageCooldown), 0.1f);
        }
    }


    private void DamageCooldown()
    {
        player.TakeDamage(damage);
        canDamage = true;
    }

}

