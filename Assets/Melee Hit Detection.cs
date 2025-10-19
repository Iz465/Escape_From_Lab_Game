using UnityEngine;

public class MeleeHitDetection : MonoBehaviour
{
   
    private navmeshtestscript enemy;
    public static bool canTrigger;
    public static float damage;


    private void Start()
    {
        canTrigger = false;
    }
    private void OnTriggerEnter(Collider other)
    {
    /*
        enemy = other.GetComponentInParent<navmeshtestscript>();
        if (!enemy)
            enemy = other.GetComponent<navmeshtestscript>();
        if (!enemy) return;
        if (!canTrigger) return;
        Debug.Log($"Trigger: {other.name}");
        canTrigger = false;
        enemy.TakeDamage(damage); */
    }



}
