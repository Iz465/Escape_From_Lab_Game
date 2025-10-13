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
        Debug.Log($"Power hit: {collision.gameObject}");
        enemy = collision.gameObject.GetComponent<navmeshtestscript>();
        ApplyDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Power hit: {other.gameObject}");
        enemy = other.gameObject.GetComponent<navmeshtestscript>();
        ApplyDamage();
    }


    private void ApplyDamage()
    {

        //      iCollide = GetComponent<ICollide>();

        if (!enemy)
        {
            //   Debug.Log("Hit something");
            poolManager.ReleaseToPool(gameObject);
            return;
        }

        enemy.TakeDamage(power.stats.damage);
        poolManager.ReleaseToPool(gameObject);
    }

}


//  if (iCollide != null)
//   {
//       Debug.Log("Hit Enemy");
//       enemy.TakeDamage(power.stats.damage);       
//       iCollide.CollideResult(collision.collider, gameObject);
//  }


//  if (iCollide == null)
//   {
//       Debug.Log("Hit Enemy");
//       enemy.TakeDamage(power.stats.damage);
//       poolManager.ReleaseToPool(gameObject);
//    }