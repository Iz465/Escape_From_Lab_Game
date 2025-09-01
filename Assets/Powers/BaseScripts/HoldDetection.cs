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
        takeDamage = collision.gameObject.GetComponent<IDamageTaken>();
        if (takeDamage != null)
            takeDamage.TakeDamage(power.stats.damage);
    }
}
