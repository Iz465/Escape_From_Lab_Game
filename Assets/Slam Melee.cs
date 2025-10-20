using UnityEngine;
using UnityEngine.InputSystem;

public class SlamMelee : BasePower
{
    public override void StartAttack(InputAction.CallbackContext context)
    {
      
        base.StartAttack(context);
        Attack();
    }



}
