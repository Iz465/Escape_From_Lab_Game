using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    IDamageTaken takeDamage;
    [SerializeField]
    PowerData powerData;
    ICollide iCollide;
    private int num;
    ObjectPoolManager poolManager;

    private void Awake()
    {
        
        num = 0;
    }

    private void Start()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
        if (!poolManager)
            Debug.Log("pool manager null");

    }



    private void OnCollisionEnter(Collision collision)
    {

            

        takeDamage = collision.gameObject.GetComponent<IDamageTaken>();
            
        if (takeDamage == null)
        {
            poolManager.ReleaseToPool(powerData.prefab, gameObject);
            return;
        }

      

        iCollide = GetComponent<ICollide>();
        if (iCollide != null)
        {
            num++;
            if (num >= 2) poolManager.ReleaseToPool(powerData.prefab, gameObject);
            iCollide.CollideResult(collision.collider, gameObject);
        }

    


    }

    private void OnCollisionStay(Collision collision)
    {
       // if (takeDamage != null)
     //       takeDamage.TakeDamage(powerData.damage, gameObject);




    }






}


