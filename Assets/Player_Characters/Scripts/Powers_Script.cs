using UnityEngine;
using UnityEngine.InputSystem;

public class Powers_Script : MonoBehaviour, IAttack
{

    public PowerData powerData;
    protected Transform boxAim;
    protected Rigidbody powerBody;
    protected GameObject powerInstance;
    protected Camera cam;
    [SerializeField]
    protected PlayerData playerData;



 


    protected void Start()
    {
      
        cam = GetComponentInChildren<Camera>();
        boxAim = transform.Find("BoxAim");
    }



    virtual public void Attack(InputAction.CallbackContext context) 
    {
        if (context.performed && consumeStamina()) 
        {
         
            if (powerData.powerVFX != null)
            {
                
       
                Vector3 aimDir = cam.transform.forward;
                powerInstance = Instantiate(powerData.powerVFX, boxAim.position, Quaternion.LookRotation(aimDir));

           
                powerBody = powerInstance.GetComponent<Rigidbody>();
                Destroy(powerInstance, powerData.duration);
                if (powerBody != null)
                {

                    powerBody.sleepThreshold = 0f;  // makes CollisionOnStay last forever as body doesnt go to sleep.
                    powerBody.AddForce(aimDir * powerData.speed, ForceMode.Impulse); // Powers go where player aims.
                    

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
   
        if (playerData.stamina < powerData.stamina)
            return false;

        playerData.stamina -= powerData.stamina;
        playerData.stamina = Mathf.Clamp(playerData.stamina, 0, playerData.maxStamina);
   
        return true;
    } 
}


