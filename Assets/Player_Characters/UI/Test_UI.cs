using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider abilityOneSlider;

    [SerializeField]
    private Player player;

    private void Start()
    {
        if (!player)
        {
            player = FindAnyObjectByType<Player>();
            if (!player)
                Debug.Log("CANT FIND PLAYER");
        }
      
    }


    private void Update()
    {
        if (player)
        {
            if (healthText) healthText.text = $"Health : {player.stats.health}";

            if (healthBar) healthBar.value = player.stats.health;
         
            if (abilityOneSlider) abilityOneSlider.value = Player.abilityCooldown;

        } 



    }
}
