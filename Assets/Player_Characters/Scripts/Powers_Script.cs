using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Powers_Script : MonoBehaviour
{

    public PowerData powerData;
    private Transform boxAim;
    private Rigidbody powerBody;
    protected GameObject powerInstance;
    protected Camera cam;
    [SerializeField]
    protected PlayerData playerData;
    public ObjectPoolManager poolManager;






    protected void Start()
    {


        cam = GetComponentInChildren<Camera>();
        if (!cam)
            Debug.LogWarning("No cam");
        boxAim = transform.Find("BoxAim");
        if (!boxAim)
            Debug.LogWarning("No boxAim");


    }



    virtual public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && consumeStamina())
        {

            if (!powerData.powerVFX) return;
                       
            Vector3 aimDir = cam.transform.forward;
                
            // hide/unhide objects instead of spawning them to save memory.
            powerInstance = poolManager.SpawnFromPool(powerData.powerVFX, boxAim.position, Quaternion.LookRotation(aimDir));

            if (powerInstance == null) return;
                

            powerBody = powerInstance.GetComponent<Rigidbody>();
            StartCoroutine(hidePower(powerData.duration, powerInstance));
            if (powerBody != null)
            {
                powerBody.sleepThreshold = 0f;  // makes CollisionOnStay last forever as body doesnt go to sleep.
                powerBody.AddForce(aimDir * powerData.speed, ForceMode.Impulse); // Powers go where player aims.
            }
                
            else
                Debug.LogWarning("Body not found");

        }
    }



    private IEnumerator hidePower(int time, GameObject disablePower)
    {
        yield return new WaitForSeconds(time);
        poolManager.ReleaseToPool(powerData.powerVFX, disablePower);
        Debug.Log("hidden");
    }

    virtual protected bool consumeStamina()
    {
        /*
        if (playerData.stamina < powerData.stamina)
            return false;

        playerData.stamina -= powerData.stamina;
        playerData.stamina = Mathf.Clamp(playerData.stamina, 0, playerData.maxStamina);
        */
        return true;
    }
}


