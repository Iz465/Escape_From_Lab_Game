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
        enemy = collision.gameObject.GetComponent<Enemy>();
  

        if (!enemy)
        {
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        else
            enemy.TakeDamage(power.stats.damage);
        
    
            iCollide = GetComponent<ICollide>();
        if (iCollide != null)
            iCollide.CollideResult(collision.collider, gameObject);

        else
            poolManager.ReleaseToPool(gameObject);

    }

}


