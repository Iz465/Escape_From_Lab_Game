using UnityEngine;
using UnityEngine.InputSystem;

public class ArcSwing : BasePower
{

    public override void StartAttack(InputAction.CallbackContext context)
    {

        base.StartAttack(context);
        Attack();
    }

    private void ResetRotation()
    {

       
    }


}
