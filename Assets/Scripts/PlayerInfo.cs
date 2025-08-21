using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float health, stamina;
    public float lastDamageTime;
    public float maxHealth;

    void Start()
    {
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
