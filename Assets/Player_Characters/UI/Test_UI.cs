using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField] private Slider healthBar;

    [SerializeField]
    private Text staminaText;
    [SerializeField] private Slider staminaBar;



    [SerializeField] private Slider abilityOneSlider;
    [SerializeField] private Speed speed;

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

        if (!speed)
            speed = FindAnyObjectByType<Speed>();

      
    }


    private void Update()
    {
        if (player)
        {
            if (healthText) healthText.text = $"Health : {player.stats.health}";

            if (healthBar) healthBar.value = player.stats.health;
         
            if (abilityOneSlider) abilityOneSlider.value = Player.abilityCooldown;

        }

        else if(speed)
        {
            if (healthText) healthText.text = $"Health : {speed.health}";

            if (healthBar) healthBar.value = speed.health;

            if (staminaText) staminaText.text = $"Stamina : {speed.stamina}";

            if (staminaBar) staminaBar.value = speed.stamina;
        }

   

    }
}
