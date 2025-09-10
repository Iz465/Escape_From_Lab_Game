using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
            player.TakeDamage(damage);
        Destroy(gameObject);
    }

}
