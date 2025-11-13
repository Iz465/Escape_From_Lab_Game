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
        Player.ability2Cooldown = 0;

        while (Player.ability2Cooldown < time)
        {
            Player.ability2Cooldown += Time.deltaTime;
            yield return null;
        }
        Debug.Log("IMMORTALITY RESET");
        Player.ability2Cooldown = 15;
        canCast = true;

    }

}
