using UnityEngine;

public class PlayerData: ScriptableObject
{
    public string playerName;
    public float health;
    public float maxHealth;
    public float stamina;
    public float maxStamina;
    public int dmgMultiplier;

    public void ResetStats()
    {
        health = maxHealth;
        stamina = maxStamina;
    }
}
