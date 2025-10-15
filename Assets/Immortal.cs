using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Immortal : MonoBehaviour
{
    [SerializeField] private float powerLength;
    [SerializeField] private ParticleSystem immortalParticle;
    private ParticleSystem immortalParticleInstance;
  

    public void ActiveImmortality(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Kael_Draven.canDamage = false;
        immortalParticleInstance = Instantiate(immortalParticle, transform.position, transform.rotation);
        immortalParticleInstance.transform.parent = transform;
        StartCoroutine(ImmortalLength(powerLength));

    }

    private IEnumerator ImmortalLength(float timer)
    {
        yield return new WaitForSeconds(timer);
        Kael_Draven.canDamage = true;
        immortalParticleInstance.Stop();
    }

}
