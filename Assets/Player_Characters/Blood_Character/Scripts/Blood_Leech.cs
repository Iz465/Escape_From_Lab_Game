using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Leech : Powers_Script 
{

   public override void Initialize()
    {
        powerName = "Leeching Blood!";
        damage = 10;
        stamina = 5; 
    }

  
}

/*

Player player;
 
player = GetComponent<Player>();

if (player != null)
    player.playerHurt();
else
    Debug.Log("Player not attatched!"); */