using UnityEngine;
using UnityEngine.InputSystem;

public class InstaKill : BasePower
{
    BloodPowerData bloodData;


    // this ability will insta kill all enemies under 50% hp. also ricochets to another enemy if it hits an enemy.
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;

    }


    
    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        playerData.health -= bloodData.loseHealth;
        playerData.health = Mathf.Clamp(playerData.health, 10, playerData.maxHealth);


        return base.UseStamina();
    }

}
