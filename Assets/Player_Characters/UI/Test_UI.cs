using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text staminaText;
    Player player;

    private void Awake()
    {
        player = Object.FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if (healthText)
            healthText.text = $"Health : {Player.health}";
        if (staminaText)
            staminaText.text = $"STamina : {player.stamina}";

    }
}
