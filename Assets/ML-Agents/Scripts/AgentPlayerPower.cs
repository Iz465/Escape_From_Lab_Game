using System.Collections;
using UnityEngine;

public class AgentPlayerPower : MonoBehaviour
{


    bool doOnce = true;

    [SerializeField] private GameObject playerAttack;
    [SerializeField] private GameObject enemy;

    private void Update()
    {
        
        if (doOnce)
        {
            doOnce = false;
            Attack(3);
        }    
    }
    private IEnumerator Attack(float time)
    {

        while (true)
        {

            GameObject playerInstance = Instantiate(playerAttack, transform.position, transform.rotation);

            if (!playerInstance) yield break;
            Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
            if (!rb) yield break;
            Collider collider = enemy.GetComponent<Collider>();
            Vector3 aimDir = (collider.bounds.center - transform.position).normalized;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.AddForce(aimDir * 80, ForceMode.Impulse);

            yield return new WaitForSeconds(time);

        }
       
    }




    

}
