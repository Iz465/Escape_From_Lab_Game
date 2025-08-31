using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    public PlayerData playerData;

    private void Awake()
    {
        if (playerData) 
            playerData.ResetStats();
        
        else 
            Debug.LogWarning("No player data");
    }

    private void Update()
    {
        if (!playerData) return;
        playerData.stamina += 5f * Time.deltaTime;
        playerData.stamina = Mathf.Clamp(playerData.stamina, 0, playerData.maxStamina);
    }


    public void TakeDamage(float damageTaken) 
    {
        playerData.health -= damageTaken;
        if (playerData.health <= 0)
            playerDeath();
    }


    public void playerDeath() 
    {
        Debug.Log("You have died");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log($"Controller hit something : {hit.gameObject}");
    }

}
