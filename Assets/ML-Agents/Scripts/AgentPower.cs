using UnityEngine;

public class AgentPower : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
    
        Debug.Log($" POWER HAS COLLIDED WITH: {collision.gameObject}");

        if (collision.gameObject.tag == "Player")
        {
            AgentMage mage = FindAnyObjectByType<AgentMage>();

            if (mage)
                mage.HitPlayer();

            Destroy(gameObject);
        }
    }

}
