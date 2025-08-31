using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    protected IDamageTaken takeDamage;
    [SerializeField]
    protected PowerData powerData;
    protected ICollide iCollide;
    private ObjectPoolManager poolManager;


    private void Awake()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
    }



    virtual protected void OnCollisionEnter(Collision collision)
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
        else
            poolManager.ReleaseToPool(powerData.prefab, gameObject); 

    }


   
    






}


