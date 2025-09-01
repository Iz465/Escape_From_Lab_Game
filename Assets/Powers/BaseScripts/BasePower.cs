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

    [System.Serializable]

    public struct PowerStats
    {
        public GameObject prefab;
        public float damage;
        public float speed;
        public float stamina;
    }

    public PowerStats stats;

    [Serializable]
    protected enum PowerType
    {
        Shoot,
        Hold,
        Spawn,
        Melee
    }
    protected PowerType powerType;

    private void Start()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();
    
    }

    virtual public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!PowerChecks()) return;
        if (!powerInstance) return;

  /*      switch case(powerType)
        {
            case PowerType.Shoot: break;
            case PowerType.Hold: break;
            case PowerType.Spawn: break;
            case PowerType.Melee: break;
            default: break;

        } */

        FirePower(powerInstance);
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
        rb.AddForce(cam.transform.forward * stats.speed, ForceMode.Impulse);
    }

   virtual protected bool UseStamina()
    {
        if (player.stats.stamina < stats.stamina) return false;

        player.stats.stamina -= stats.stamina;
        player.stats.stamina = Mathf.Clamp(player.stats.stamina, 0, player.maxStamina);
        return true;
    }

    protected IEnumerator DestroyPower(int time, GameObject power)
    {
        yield return new WaitForSeconds(time);
        poolManager.ReleaseToPool(power);
    }


}
