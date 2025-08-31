using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BasePower : MonoBehaviour
{
    [SerializeField]
    protected PowerData powerData;
    [SerializeField]
    protected PlayerData playerData;
    protected GameObject powerInstance;
    protected  ObjectPoolManager poolManager;
    [SerializeField]
    protected Transform boxAim;
    [SerializeField]
    protected Camera cam;

    protected Rigidbody rb;

  
    private void Start()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    virtual public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!PowerChecks()) return;
        if (!powerInstance) return;
        FirePower(powerInstance);
    }


    protected bool PowerChecks()
    {
        if (!powerData.prefab)
        {
            Debug.LogWarning("Power Prefab Not Found");
            return false;
        }
        if (!UseStamina()) return false;

        if (!boxAim)
        {
            Debug.LogWarning("BoxAim Not Found");
            return false;
        } 

        if (!cam)
        {
            Debug.LogWarning("Camera Not Found");
            return false;
        }
        if (!poolManager) Debug.LogWarning("no pool");
        powerInstance = poolManager.SpawnFromPool(powerData.prefab, boxAim.position, Quaternion.LookRotation(cam.transform.forward));

        return true;
    }



    virtual protected void FirePower(GameObject power)
    {
        rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogWarning("Power has no rigidbody");
            return;
        }
        rb.sleepThreshold = 0;
        rb.AddForce(cam.transform.forward * powerData.speed, ForceMode.Impulse);
    }

   virtual protected bool UseStamina()
    {
        if (playerData.stamina < powerData.stamina) return false;

        playerData.stamina -= powerData.stamina;
        playerData.stamina = Mathf.Clamp(playerData.stamina, 0, playerData.maxStamina);
        return true;
    }

    protected IEnumerator DestroyPower(int time, GameObject power)
    {
        yield return new WaitForSeconds(time);
        poolManager.ReleaseToPool(powerData.prefab, power);
    }


}
