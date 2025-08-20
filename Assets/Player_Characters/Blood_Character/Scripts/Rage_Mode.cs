using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rage_Mode : Powers_Script
{


    public override void Attack(InputAction.CallbackContext context)
    {
      
        if (context.performed)
        {
            if (powerVFX)
            {
                Debug.Log("Rage Activated");
                powerInstance = Instantiate(powerVFX);
                Destroy(powerInstance, 5f);
            }
     
        }
     
    }

  

}
