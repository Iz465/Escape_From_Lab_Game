using UnityEngine;
using UnityEngine.InputSystem;

public class FreezeParticleDetection : MonoBehaviour
{
    [SerializeField] private GameObject freezeParticle;



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("FREEZE HIT");
        ContactPoint hit = collision.contacts[0]; // gets the exact location the particle hit.
        Instantiate(freezeParticle, hit.point, Quaternion.identity); 

        navmeshtestscript enemy = collision.gameObject.GetComponent<navmeshtestscript>();
        if (enemy)
        {
            WarriorFreeze warriorFreeze = FindAnyObjectByType<WarriorFreeze>();
            if (warriorFreeze)
            {
                Debug.Log("FOUND WARRIOR FREEZE");
                warriorFreeze.FreezeEnemy(enemy);
            }
        }

     
        
        Destroy(gameObject);
    }

}
