using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    public PlayerData playerData;

    private void Awake()
    {
        if (playerData) playerData.ResetStats();

        else Debug.LogWarning("No player data");

    }

    private void Update()
    {
        if (!playerData) return;
        playerData.stamina += 5f * Time.deltaTime;
        playerData.stamina = Mathf.Clamp(playerData.stamina, 0, playerData.maxStamina);
    }


    public void takeDamage(float damageTaken, GameObject weapon) 
    {
        playerData.health -= damageTaken;
        Debug.Log($"Health = {playerData.health}");
        if (playerData.health <= 0)
            playerDeath();
    }


    public void playerDeath() 
    {
        Debug.Log("You have died");
    }

    
}
