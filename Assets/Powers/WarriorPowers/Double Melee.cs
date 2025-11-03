using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleMelee : BasePower
{

    public override void StartAttack(InputAction.CallbackContext context)
    {
        MeleeHitDetection.damage = stats.damage;
        base.StartAttack(context);
        Attack();
    }

    private void FirstDoubleMelee()
    {
        MeleeHitDetection.canTrigger = true;    
    }

    private void SecondDoubleMelee()
    {
        MeleeHitDetection.canTrigger = true;
    }

}
