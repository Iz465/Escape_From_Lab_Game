using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Leech : Powers_Script, IGetHealth
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
            //  powerInstance = Instantiate(powerVFX, player.transform.position + distance, player.transform.rotation);
            Vector3 spawnPos = player.transform.position + player.transform.forward * 2f;
            Quaternion spawnRot = Quaternion.LookRotation(player.transform.forward);

            powerInstance = Instantiate(powerVFX, spawnPos, spawnRot);
            powerInstance.AddComponent<Power_Hit_Detection>(); // adds this script to the spawned powers. So I don't have to add manually in editor.

            body = powerInstance.GetComponent<Rigidbody>();

            if (body != null)
            {
                body.AddForce(transform.up * 10f, ForceMode.Impulse); // Makes the powers move towards player aim. (needs to be set up to aim where player is aiming)
                Destroy(powerInstance, 5f);

            }
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

            Debug.Log($"Damage: {damage}");

            player.stamina -= stamina;
        }
           
        else
        {
            Destroy(powerInstance);
            return false;
        }
   
            return true;
    }

    public void GetHealth()
    {
        Debug.Log("GAINING HEALTH");
        Player.health += 1;
        Player.health = Mathf.Clamp(Player.health, 0, 100);
    }

}
