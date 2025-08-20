using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Leech : Powers_Script 
{
    static bool isHeld;

    protected void Update()
    {
        if (isHeld)
            consumeStamina();
    }

    public override void Attack(InputAction.CallbackContext context)
    {
        if (context.started && consumeStamina()) 
        {
            isHeld = true;
     
            Vector3 distance = new Vector3(0, 2, 0);
            powerInstance = Instantiate(powerVFX, player.transform.position + distance, player.transform.rotation);

        }

        if (context.canceled)
        {
            isHeld = false;
            Destroy(powerInstance);
        }
    }

    protected override bool consumeStamina()
    {
        player = Object.FindFirstObjectByType<Player>();
       


        if (player.stamina > 0)
        {
            Debug.Log($"Stamina left: {player.stamina}");
            player.stamina -= stamina;
        }
           
        else
        {
            Destroy(powerInstance);
            return false;
        }
   
            return true;
    }

}
