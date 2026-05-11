using UnityEngine;

public class BloodPool : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private AudioClip drinkSound;
    [SerializeField] private AudioSource audioSource;

  

    private void OnTriggerEnter(Collider other)
    {
        if (player == other.GetComponent<Player>())
        {
            Debug.Log("Blood Fountain Entered!");
            player.stats.health += 20;
            player.stats.health = Mathf.Clamp(player.stats.health, 0, 100);
            Destroy(gameObject);
        }

        if (drinkSound)
            audioSource.PlayOneShot(drinkSound);
  
    }
}
