using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Leech : Powers_Script, IGetHealth
{
    static bool isHeld;
    private float spawnLoc;

    protected void Update()
    {
        if (isHeld)
        consumeStamina();
    }

    private void Awake()
    {
        spawnLoc = 5f;
    }

    public override void Attack(InputAction.CallbackContext context)
    {
        base.Attack(context);

 
        if (context.performed && powerInstance != null)
        {
            isHeld = true;
            powerInstance.transform.position += cam.transform.forward * spawnLoc;
        }
         
        if (context.canceled)
        {
            isHeld = false;
            poolManager.ReleaseToPool(powerData.powerVFX, powerInstance);
        }

    }

    protected override bool consumeStamina()
    {
        if (playerData.stamina < powerData.stamina)
        {
            Destroy(powerInstance);
            isHeld = false;
        }
          
        return base.consumeStamina();
    }

    public void GetHealth()
    {
        Debug.Log("GAINING HEALTH");
        playerData.health += 1;
        playerData.health = Mathf.Clamp(playerData.health, 0, 100);
    }

}
