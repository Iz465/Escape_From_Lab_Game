using UnityEngine;

public class BloodPool : MonoBehaviour
{
    [SerializeField] private Player player;



    private void OnTriggerEnter(Collider other)
    {

        player.stats.health += 20;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, 100);
        Destroy(gameObject);
    }
}
