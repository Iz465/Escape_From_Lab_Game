using UnityEngine;

public class BloodPool : MonoBehaviour
{
    [SerializeField] private Player player;



    private void OnTriggerEnter(Collider other)
    {
        if (!player == other.gameObject) return;
        Debug.Log("Blood Fountain Entered!");
        player.stats.health += 20;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, 100);
        Destroy(gameObject);
    }
}
