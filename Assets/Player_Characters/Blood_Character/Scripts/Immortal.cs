using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Immortal : MonoBehaviour
{
    [SerializeField] private float powerLength;
    [SerializeField] private ParticleSystem immortalParticle;
    private ParticleSystem immortalParticleInstance;
    private bool canCast = true;


    public void ActiveImmortality(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canCast) return;

        Kael_Draven.canDamage = false;
        immortalParticleInstance = Instantiate(immortalParticle, transform.position, transform.rotation);
        immortalParticleInstance.transform.parent = transform;
        StartCoroutine(ImmortalLength(powerLength));
        canCast = false;

    }

    private IEnumerator ImmortalLength(float timer)
    {
        yield return new WaitForSeconds(timer);
        Kael_Draven.canDamage = true;
        immortalParticleInstance.Stop();
        StartCoroutine(ResetCast(15f));

    }

    private IEnumerator ResetCast(float time)
    {
        yield return new WaitForSeconds(time);
        canCast = true;
    }

}
