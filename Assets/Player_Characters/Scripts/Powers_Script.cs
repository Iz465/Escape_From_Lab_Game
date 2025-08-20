using UnityEngine;
using UnityEngine.InputSystem;

public class Powers_Script : MonoBehaviour, IAttack
{
    public int damage;
    public float stamina;
    public string powerName;
    public GameObject powerVFX;
    protected Rigidbody body;
    static protected GameObject powerInstance;
    protected Player player;

    virtual protected void Awake()
    {
        player = Object.FindFirstObjectByType<Player>();
        damage *= Player.multiplier;
        Debug.Log($"ACTIVE DAMAGE IS {damage}");
        
  
    }

    virtual public void Attack(InputAction.CallbackContext context) 
    {
        if (context.performed && consumeStamina()) 
        {

            if (powerVFX != null)
            {
                //    Debug.Log($"{powerVFX.name} has been cast");

                Vector3 spawnPos = player.transform.position + player.transform.forward * 2f;
                Quaternion spawnRot = Quaternion.LookRotation(player.transform.forward);
                
                powerInstance = Instantiate(powerVFX, spawnPos, spawnRot);
                powerInstance.AddComponent<Power_Hit_Detection>(); // adds this script to the spawned powers. So I don't have to add manually in editor.

                body = powerInstance.GetComponent<Rigidbody>(); 

                if (body != null)
                {
                    body.AddForce(transform.forward * 10f, ForceMode.Impulse); // Makes the powers move towards player aim. (needs to be set up to aim where player is aiming)
                    Destroy(powerInstance, 5f); 

                }

                else
                    Debug.Log("Body not found");

            }

            else
                Debug.Log("Power can't be found");

        }
    }


    virtual protected bool consumeStamina()
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

 


}
