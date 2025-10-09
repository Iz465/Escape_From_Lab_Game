using Unity.VisualScripting;
using UnityEngine;

public class Power_Hit_Detection : MonoBehaviour
{
    
    [SerializeField]
    protected BasePower power;
    protected ICollide iCollide;
    private ObjectPoolManager poolManager;
    protected navmeshtestscript enemy;
    

    private void Awake()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
   
    }


    virtual protected void OnCollisionEnter(Collision collision)
    {
        enemy = collision.gameObject.GetComponent<navmeshtestscript>();
        iCollide = GetComponent<ICollide>();

        Debug.Log("Hit something");
            
        if (!enemy)
        {
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        if (iCollide != null)
        {
            Debug.Log("Hit Enemy");
            enemy.TakeDamage(power.stats.damage);
            iCollide.CollideResult(collision.collider, gameObject);
        }
          

        if (iCollide == null)
        {
            Debug.Log("Hit Enemy");
            enemy.TakeDamage(power.stats.damage);
            poolManager.ReleaseToPool(gameObject);
        }
     
    }

}


