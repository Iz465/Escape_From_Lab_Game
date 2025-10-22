using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockAttacks : MonoBehaviour
{
    [System.Serializable] public struct ParticleTypes
    {
        public GameObject redParticle;
        public GameObject greenParticle;
        public GameObject blueParticle;
    }

    public enum ParticleInUse
    {
      red,
      green,
      blue,
      none
    }

    [HideInInspector] public static ParticleInUse particleInUse;

    [SerializeField] private ParticleTypes particleTypes = new ParticleTypes();
    private Collider playerCollider;
    private GameObject[] currentParticle = new GameObject[1];

     

    private void Start()
    {
        playerCollider = GetComponentInParent<Collider>();
        particleInUse = ParticleInUse.none;
    }
    public void BlockRed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        SwitchParticle(particleTypes.redParticle);
        particleInUse = ParticleInUse.red;
    }

    public void BlockGreen(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        SwitchParticle(particleTypes.greenParticle);
        particleInUse = ParticleInUse.green;
    }

    public void BlockBlue(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        SwitchParticle(particleTypes.blueParticle);
        particleInUse = ParticleInUse.blue;
    }

    private void SwitchParticle(GameObject particle)
    {
        GameObject newParticle = Instantiate(particle, playerCollider.bounds.center + new Vector3 (0 , -1, 0), transform.rotation);
        if (currentParticle[0])
            Destroy(currentParticle[0]);
        currentParticle[0] = newParticle;
        currentParticle[0].transform.parent = transform;
       
    }

}
