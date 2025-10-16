using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleMelee : BasePower
{

    public override void StartAttack(InputAction.CallbackContext context)
    {
        base.StartAttack(context);
        Attack();
    }


}
