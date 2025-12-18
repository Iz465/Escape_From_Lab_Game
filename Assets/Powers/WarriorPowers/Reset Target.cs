using UnityEngine;
using UnityEngine.InputSystem;

public class ResetTarget : MonoBehaviour
{


    public void ActivateReset(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        ArcSwing.enemyHit[0] = null;
        Debug.Log("ENEMY TARGET RESET");

    }
    
}
