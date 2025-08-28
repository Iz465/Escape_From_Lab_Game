using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    private IDamageTaken takeDamage;
    [SerializeField]
    private PowerData powerData;
    private ICollide iCollide;
    private ObjectPoolManager poolManager;


    private void Awake()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        takeDamage = collision.gameObject.GetComponent<IDamageTaken>();

        if (takeDamage == null)
        {
            poolManager.ReleaseToPool(powerData.prefab, gameObject);
            return;
        }

        else
            takeDamage.TakeDamage(powerData.damage);

            iCollide = GetComponent<ICollide>();
        if (iCollide != null)     
            iCollide.CollideResult(collision.collider, gameObject);
   
    }


    private void OnCollisionStay(Collision collision)
    {
       // if (takeDamage != null)
     //       takeDamage.TakeDamage(powerData.damage, gameObject);




    }






}


