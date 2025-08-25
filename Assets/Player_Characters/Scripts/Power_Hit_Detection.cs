using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    IDamageTaken takeDamage;
    [SerializeField]
    PowerData powerData;
    IGetHealth getHealth;
    Blood_Leech bloodLeech;
 




    private void OnCollisionEnter(Collision collision)
    {
       
        // Only objects that take damage (players, enemies etc) will have the IDamageTaken interface
        takeDamage = collision.gameObject.GetComponent<IDamageTaken>();
        if (takeDamage != null)
            takeDamage.takeDamage(powerData.damage, gameObject);
        else
            Debug.Log("Hit something");
      
        if (!powerData.hold)
            Destroy(gameObject);
        
            
    }

    private void OnCollisionStay(Collision collision)
    {
        if (takeDamage != null)
            takeDamage.takeDamage(powerData.damage, gameObject);




    }






}


