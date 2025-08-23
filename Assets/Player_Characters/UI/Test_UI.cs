using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text staminaText;
    [SerializeField]
    PlayerData playerData;

   

    private void Update()
    {
        if (playerData != null)
        {
            if (healthText)
                healthText.text = $"Health : {playerData.health}";
            if (staminaText)
                staminaText.text = $"Stamina : {playerData.stamina}";
        }

    }
}
