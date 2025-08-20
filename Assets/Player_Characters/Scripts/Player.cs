using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    public static int health;
    public float stamina;
    public float maxStamina;

   
    public void takeDamage(int damageTaken) 
    {
        health -= damageTaken;
        Debug.Log($"Health = {health}");
        if (health <= 0)
            playerDeath();
    }


    public void playerDeath() 
    {
        Debug.Log("You have died");
    }

    
}
