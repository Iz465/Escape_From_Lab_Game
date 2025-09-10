using UnityEngine;
using UnityEngine.InputSystem;

public class HoldPower : BasePower
{
   protected static bool isHeld;
   virtual protected void Update()
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
            poolManager.ReleaseToPool(powerInstance);
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

   
   
}
