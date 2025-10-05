using UnityEngine;

public class BloodLevel : MonoBehaviour
{
    [SerializeField] private Player player;

    void Start()
    {
        
    }


    void Update()
    {
        player.stats.health -= 1f * Time.deltaTime;

        
    }
}
