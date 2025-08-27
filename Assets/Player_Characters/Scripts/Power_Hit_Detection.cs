using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    IDamageTaken takeDamage;
    IGetHealth getHealth;
    [SerializeField]
    PowerData powerData;
    ICollide iCollide;
 




    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Hit");

        iCollide = GetComponent<ICollide>();
        if (iCollide != null)
            iCollide.CollideResult(gameObject);

        // Only objects that take damage (players, enemies etc) will have the IDamageTaken interface
        //      takeDamage = collision.gameObject.GetComponent<IDamageTaken>();
        //      if (takeDamage != null)
        //       takeDamage.takeDamage(powerData.damage, gameObject);
        //   else
        //     Debug.Log("Hit something");

        //     if (!powerData.hold)
        //        Destroy(gameObject);


    }

    private void OnCollisionStay(Collision collision)
    {
        if (takeDamage != null)
            takeDamage.takeDamage(powerData.damage, gameObject);




    }






}


