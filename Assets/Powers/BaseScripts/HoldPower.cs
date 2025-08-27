using UnityEngine;
using UnityEngine.InputSystem;

public class HoldPower : BasePower
{
    static bool isHeld;
    private void Update()
    {
        if (isHeld) UseStamina();
       
    }

    public override void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PowerChecks();
            if (!powerInstance) return;
            FirePower(powerInstance);
            isHeld = true;
        }
       

        if (context.canceled)
        {
            isHeld = false;
            StartCoroutine(DestroyPower(powerData.duration, powerInstance));
        }
       
    }

    protected override void FirePower(GameObject power)
    {
        rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogWarning("Power has no rigidbody");
            return;
        }
        rb.sleepThreshold = 0;
    }

    protected override bool UseStamina()
    {

        if (!playerData || !powerData) return false;
        if (playerData.stamina < powerData.stamina)
        {
            isHeld = false;
            StartCoroutine(DestroyPower(powerData.duration, powerInstance));
            return false;
        } 

        return base.UseStamina();
    }

}
