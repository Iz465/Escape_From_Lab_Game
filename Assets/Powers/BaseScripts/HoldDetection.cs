using UnityEngine;

public class HoldDetection : Power_Hit_Detection
{

    protected override void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {

        iCollide = GetComponent<ICollide>();
        if (iCollide != null)
            iCollide.CollideResult(collision.collider, gameObject);
        enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
            enemy.TakeDamage(power.stats.damage);
    }
}
