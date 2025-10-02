using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Something has been hit : {collision.gameObject}");
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
            player.TakeDamage(damage);
        Destroy(gameObject);
    }

}

