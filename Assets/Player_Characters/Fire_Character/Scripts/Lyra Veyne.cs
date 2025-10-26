using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;

[CreateAssetMenu(fileName = "LyraVeyne", menuName = "Scriptable Objects/LyraVeyne")]
public class LyraVeyne : ScriptableObject
{

    public string characterName = "Lyra Veyne";

    [Range(0, 100)]
    public int health;
    [Range(0, 100)]
    public float Stamina;


    public float sprintSpeed = 8f;
    public float walkSpeed = 5f;
    public float jumpHeight = 2f;
    public float rotationSpeed = 720f;


    public string DisplayStamina;

}


