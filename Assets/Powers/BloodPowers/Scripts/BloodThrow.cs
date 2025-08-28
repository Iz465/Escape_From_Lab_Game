using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodThrow : SpawnPower
{
    private BloodPowerData bloodData;



    

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



