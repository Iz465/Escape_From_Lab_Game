using UnityEngine;

[CreateAssetMenu(fileName = "LyraVeyne", menuName = "Scriptable Objects/LyraVeyne")]
public class LyraVeyne : ScriptableObject
{
    public string characterName = "Lyra Veyne";
    public int health = 100;
    public int Stamina = 50;
    
    public const float sprintSpeed = 8f;
    public const float walkSpeed = 5f;
    public float jumpHeight = 2f;

}
