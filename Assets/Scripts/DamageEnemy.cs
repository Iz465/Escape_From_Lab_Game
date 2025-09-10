using UnityEngine;
using UnityEngine.UIElements;

public class DamageEnemy : MonoBehaviour
{
    public int damageAmount = 10;

    void Start()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        
    }

    
}
