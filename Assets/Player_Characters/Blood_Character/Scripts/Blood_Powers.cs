using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Powers : Powers_Script
{


    public override void Attack(InputAction.CallbackContext context)
    {
        base.Attack(context);
      
        if (playerData != null)
        {
       //     Debug.Log(playerData.health);
            if (playerData.health > 10)
                playerData.health -= powerData.healthDrain;
            playerData.health = Mathf.Clamp(playerData.health, 10, 100);
        }


    }



}
