using UnityEngine;

public class Player : MonoBehaviour
{
    protected static int health;
    protected int stamina;

    private void Start()
    {
        Debug.Log("STARTING");
  
    }

    public void playerHurt()
    {
        health -= 5;
        Debug.Log($"Health = {health}");
        if (health <= 0) 
            playerDeath();
    }


    protected void playerDeath() 
    {
        Debug.Log("You have died");
    }

    
}
