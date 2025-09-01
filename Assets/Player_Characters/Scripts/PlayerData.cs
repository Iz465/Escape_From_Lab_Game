using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerData: ScriptableObject
{
    public string playerName;
    [HideInInspector]
    public float health;
    public float maxHealth;
    [HideInInspector]
    public float stamina;
    public float maxStamina;
    public int dmgMultiplier;

    public void ResetStats()
    {
        health = maxHealth;
        stamina = maxStamina;
    }
}
