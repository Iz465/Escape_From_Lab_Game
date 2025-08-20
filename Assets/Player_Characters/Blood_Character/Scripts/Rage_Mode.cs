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
                Debug.Log($"Damage = {damage}");
                Player.multiplier = 2;
                Player.health -= 100;
                Debug.Log($"Damage doubled = {damage}");
                Destroy(powerInstance, 5f);
             
            }
     
        }
     
    }

  

}

/*
 * 
 *    Debug.Log("Rage Activated");
                powerInstance = Instantiate(powerVFX);

                player = Object.FindFirstObjectByType<Player>();
                move = player.GetComponent<Move>();
                if (move)
                {
                    move.walkSpeed *= 5;
                }
                else
                    Debug.Log("Move not found");

 * 
 * 
 */