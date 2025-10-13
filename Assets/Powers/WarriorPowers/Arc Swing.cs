using UnityEngine;
using UnityEngine.InputSystem;

public class ArcSwing : BasePower
{
  
    public override void StartAttack(InputAction.CallbackContext context)
    {
        MeleeHitDetection.damage = stats.damage;
        base.StartAttack(context);
        Attack();
    }

    private void CanArcSwipe()
    {
        MeleeHitDetection.canTrigger = true;
    }
    


}
