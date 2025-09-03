using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class BasePower : MonoBehaviour
{
    
    [SerializeField]
    protected Player player;
    protected GameObject powerInstance;
    protected  ObjectPoolManager poolManager;
    [SerializeField]
    protected Transform boxAim;
    [SerializeField]
    protected Camera cam;

    protected Rigidbody rb;
    protected static bool isHeld;


    [System.Serializable]

    public struct PowerStats
    {
        public GameObject prefab;
        public float damage;
        public float speed;
        public float stamina;
    }

    public PowerStats stats;

    [SerializeField]
    public enum PowerType
    {
        Shoot,
        Hold,
        Spawn,
        Melee
    }
    public PowerType powerType;

    private void Start()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
    
    }

    virtual public void Attack(InputAction.CallbackContext context)
    {
        if (context.canceled) isHeld = false;
        if (!context.performed) return;
        if (!PowerChecks()) return;
        if (!powerInstance) return;

        switch (powerType)
        {
            case PowerType.Shoot: ShootPower(); break;
            case PowerType.Hold:  HoldPower(context); break;
            case PowerType.Spawn: spawnPower(); break;
            case PowerType.Melee: MeleePower(); break;
            default: break;
        }

    }


    protected bool PowerChecks()
    {
        if (!stats.prefab)
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
        powerInstance = poolManager.SpawnFromPool(stats.prefab, boxAim.position, Quaternion.LookRotation(cam.transform.forward));

        if (!powerInstance)
        {
            Debug.LogWarning("No Power Instance");
            return false;
        }

            rb = powerInstance.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogWarning("Power has no rigidbody");
            return false;
        }

        return true;
    }




   virtual protected bool UseStamina()
    {
        if (player.stats.stamina < stats.stamina)
        {
            isHeld = false;
            return false;
        } 

        player.stats.stamina -= stats.stamina;
        player.stats.stamina = Mathf.Clamp(player.stats.stamina, 0, player.maxStamina);
        return true;
    }

    protected IEnumerator DestroyPower(int time, GameObject power)
    {
        yield return new WaitForSeconds(time);
        poolManager.ReleaseToPool(power);
    }

    virtual protected void HoldPower(InputAction.CallbackContext context)
    {
        isHeld = true;
        rb.sleepThreshold = 0;
    }

    virtual protected void MeleePower()
    {

    }

    virtual protected void spawnPower()
    {
        float yLoc = 15;
        float zLoc = 15;

        var powerLoc = powerInstance.transform.position;

        powerInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward);

        Vector3 forwardPos = cam.transform.forward * zLoc;
        powerLoc.z = forwardPos.z;
        powerInstance.transform.position += forwardPos;
       
        var upPos = powerInstance.transform.position;
        upPos.y = yLoc;
        powerInstance.transform.position = upPos;
        
    }

    virtual protected void ShootPower()
    {
        rb.sleepThreshold = 0;
        rb.AddForce(cam.transform.forward * stats.speed, ForceMode.Impulse);
    }

  

}
