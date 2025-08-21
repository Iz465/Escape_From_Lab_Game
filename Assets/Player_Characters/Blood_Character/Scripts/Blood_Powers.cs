using UnityEngine;

public class Blood_Powers : Powers_Script
{
    [SerializeField]
    protected int healthDrain;
    protected new void Awake()
    {
           
        if (Player.health > 10)
            Player.health -= healthDrain;
        Player.health = Mathf.Clamp(Player.health, 10, 100);
    }






}
