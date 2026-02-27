using UnityEngine;

public class AgentPlayerPowerProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log($" POWER HAS COLLIDED WITH: {collision.gameObject}");

        if (collision.gameObject.tag == "Enemy")
        {
            AgentMage mage = FindAnyObjectByType<AgentMage>();

            if (mage)
                mage.HitPlayer();

            Destroy(gameObject);
        }
    }

}
