using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rage_Mode : Blood_Powers
{

    Rage_Mode rage;
    [SerializeField]
    Image rageOverlay;
    public override void Attack(InputAction.CallbackContext context)
    {
      
        if (context.performed)
        {
            if (powerVFX)
            {
            
                powerInstance = Instantiate(powerVFX);
                rage = powerInstance.GetComponent<Rage_Mode>();
                Player.multiplier = 2;

                rage.StartCoroutine((rage.rageActive(5)));

            }
     
        }
     
    }


    IEnumerator rageActive(float time)
    {
        float timer = 0;
        float start = Player.health;
        float end = start - 50f;
        while (timer < time)
        {
            if (Player.health > 10)
                Player.health = (int)Mathf.Lerp(start, end, timer / time);
   
            timer += Time.deltaTime;
            yield return null;
        }

        Player.multiplier = 1;
        Destroy(gameObject);

    }

    /*
    IEnumerator fadeRageOut(float time)
    {
        float timer = 0;
        Color endColor = new Color(rageOverlay.color.r, rageOverlay.color.g, rageOverlay.color.b, 0f);
        Color startColor = rageOverlay.color;
        while (timer < time)
        {
            rageOverlay.color = Color.Lerp(startColor, endColor, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
   
    */

}

