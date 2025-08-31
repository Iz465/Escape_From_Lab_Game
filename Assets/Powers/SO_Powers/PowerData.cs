using UnityEngine;

[CreateAssetMenu(fileName = "NewPower", menuName = "Powers")]
public class PowerData : ScriptableObject
{
    public string power;
    public float damage;
    public float stamina;
    public float speed;
    public GameObject prefab;
    
}


