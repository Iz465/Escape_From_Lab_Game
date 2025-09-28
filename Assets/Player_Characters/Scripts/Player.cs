using UnityEngine;

public class Player : MonoBehaviour, IDamageTaken
{
    [System.Serializable]
    public struct PlayerStats
    {
        public string name;
        public float health;
        public float stamina;
    }
    public PlayerStats stats;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public float maxStamina;

    private void Awake()
    {
        maxHealth = stats.health;
        maxStamina = stats.stamina;
    }

    virtual protected void Update()
    {
        stats.stamina += 5f * Time.deltaTime;
        stats.stamina = Mathf.Clamp(stats.stamina, 0, maxStamina);
    }


    public void TakeDamage(float damageTaken) 
    {
        stats.health -= damageTaken;
        if (stats.health <= 0)
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
