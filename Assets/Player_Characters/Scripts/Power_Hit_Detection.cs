using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    IDamageTaken takeDamage;
    Powers_Script power;
    int damage;


    private void Awake()
    {
        // Checks if the power this script is being added to exists.
        power = GetComponent<Powers_Script>();
        if (power != null)
            damage = power.damage;
     
        else
            Debug.Log("Power can't be found");
    }

    private void OnCollisionEnter(Collision collision)
    {
         
       // Only objects that take damage (players, enemies etc) will have the IDamageTaken interface
        takeDamage = collision.gameObject.GetComponent<IDamageTaken>();
        if (takeDamage != null)
            takeDamage.takeDamage(damage);
        else
            Debug.Log("Hit something");

        // Destroys the power object once it hits something
            Destroy(gameObject);
    }


}


