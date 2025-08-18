using UnityEngine;
using UnityEngine.InputSystem;

public class Powers_Script : MonoBehaviour 
{
    public int damage;
    public float stamina;
    public string powerName;
    public GameObject powerVFX;
    protected Rigidbody body;
    protected GameObject powerInstance;
    private Player player;

    private void Awake()
    {
        player = Object.FindFirstObjectByType<Player>();
    }

    virtual public void activatePower(InputAction.CallbackContext context) 
    {
        if (context.performed && consumeStamina()) 
        {

            if (powerVFX != null)
            {
                Debug.Log($"{powerVFX.name} has been cast");

                powerInstance = Instantiate(powerVFX, transform.position, transform.rotation); 
                powerInstance.AddComponent<Power_Hit_Detection>(); // adds this script to the spawned powers. So I don't have to add manually in editor.

                body = powerInstance.GetComponent<Rigidbody>(); 

                if (body != null)
                {
                    body.AddForce(transform.up * 10f, ForceMode.Impulse); // Makes the powers move towards player aim.
                    Destroy(powerInstance, 5f); 

                }

                else
                    Debug.Log("Body not found");

            }

            else
                Debug.Log("Power can't be found");

        }
    }


    private bool consumeStamina()
    {
    
        player = Object.FindFirstObjectByType<Player>();
     
        if (player.stamina < stamina)
        {
            Debug.Log("Not enough stamina left!");
            return false;
        }

        player.stamina -= stamina;
        player.stamina = Mathf.Clamp(player.stamina, 0, player.maxStamina);
        Debug.Log($"Stamina left is: {player.stamina}");
        return true;
    }


    
    private void Update()
    {
        // Regenerates the players stamina over time. 

        player.stamina += 0.01f;
        player.stamina = Mathf.Clamp(player.stamina, 0, player.maxStamina);
    
    }

}
