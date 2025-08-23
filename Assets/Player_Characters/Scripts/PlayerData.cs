using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerData: ScriptableObject
{
    public string playerName;
    public float health;
    public float stamina;
    public float maxStamina;
    public int dmgMultiplier;
    
}
