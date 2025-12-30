
using UnityEngine;
using UnityEngine.Internal;

[CreateAssetMenu(fileName = "LyraVeyne", menuName = "Scriptable Objects/LyraVeyne")]
public class LyraVeyne : ScriptableObject
{

    public string characterName = "Lyra Veyne";

    [Range(0, 100)]
    public float health;
    [Range(0, 100)]
    public float Stamina;


    public float sprintSpeed = 8f;
    public float walkSpeed = 5f;
    public float jumpHeight = 2f;
    public float rotationSpeed = 720f;


    public string DisplayStamina;
    public string DisplayHealth;
    public void ResetStamina()
    {
        Stamina = 100f;
        DisplayStamina = Stamina.ToString("F0");
    }
    public void IncreaseStamina(float amount)
    {
        Stamina += amount;
        if (Stamina > 100)
        {
            Stamina = 100;
        }
        DisplayStamina = Stamina.ToString("F0");
    }
    public void ReduceStamina(float amount)
    {
        Stamina -= amount;
        if (Stamina < 0)
        {
            Stamina = 0;
        }
        UpdateDisplayValues();
    }
    public void Resethealth()
    {
        health = 100f;
        UpdateDisplayValues();
    }
    public void IncreaseHealth(float amount)
    {
        health += amount;
        if (health > 100)
        {
            health = 100;
        }
        UpdateDisplayValues();
    }
    public void ReduceHealth(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
        UpdateDisplayValues();
    }
    
    public void UpdateDisplayValues()
    {
        DisplayStamina = Stamina.ToString("F0");
        DisplayHealth = health.ToString("F0");
    }
}


