using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    public static int health;
    public float stamina;
    public float maxStamina;
    public static int multiplier;

    private void Update()
    {
        stamina += 5f * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }


    public void takeDamage(int damageTaken, GameObject weapon) 
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
