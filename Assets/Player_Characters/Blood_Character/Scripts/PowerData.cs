using UnityEngine;

[CreateAssetMenu(fileName = "NewPower", menuName = "Powers")]
public class PowerData : ScriptableObject
{
    public string powerName;
    public float damage;
    public float stamina;
    public float speed;
    public float healthDrain;
    public int duration;
    public bool hold;
    public GameObject powerVFX;
    
}


