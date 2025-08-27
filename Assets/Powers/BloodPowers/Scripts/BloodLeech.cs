using UnityEngine;

public class BloodLeech : HoldPower

{
    BloodPowerData bloodData;
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;
     
    }

    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        playerData.health += bloodData.getHealth;
        playerData.health = Mathf.Clamp(playerData.health, 0, playerData.maxHealth);


        return base.UseStamina();
    }

}
