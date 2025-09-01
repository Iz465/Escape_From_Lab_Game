using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    
    [SerializeField]
    protected BasePower power;
    protected ICollide iCollide;
    private ObjectPoolManager poolManager;
    protected Enemy enemy;


    private void Awake()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
   
    }



    virtual protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Power hitting");
        enemy = collision.gameObject.GetComponent<Enemy>();
  
        if (!enemy)
        {
            Debug.Log("Not Enemy");
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        else
            enemy.TakeDamage(power.stats.damage);
        
    
            iCollide = GetComponent<ICollide>();
        if (iCollide != null)
        {
            Debug.Log("No collide");
            iCollide.CollideResult(collision.collider, gameObject);
        }

        else
        {
            Debug.Log("Is collide");
            poolManager.ReleaseToPool(gameObject);
        }
    

    }


   
    






}


