using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private  Text staminaText;
    [SerializeField]
    private Player player;


   

    private void Update()
    {
        if (player)
        {
            if (healthText)
                healthText.text = $"Health : {player.stats.health}";
            if (staminaText)
                staminaText.text = $"Stamina : {player.stats.stamina}";
        } 

    }
}
