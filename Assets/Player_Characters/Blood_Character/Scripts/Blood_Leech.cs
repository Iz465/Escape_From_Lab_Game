using UnityEngine;
using UnityEngine.InputSystem;

public class Blood_Leech : Powers_Script, IGetHealth
{
    static bool isHeld;


    protected void Update()
    {
        if (isHeld)
        {
            consumeStamina();        
            if (powerInstance != null)
                changeSize(powerInstance);
        }
       
    }

 

    public override void Attack(InputAction.CallbackContext context)
    {
        base.Attack(context);


        if (context.performed && powerInstance != null)
        {
            isHeld = true;
            powerInstance.transform.position += cam.transform.forward;
            changeSize(powerInstance);
            var rot = powerInstance.transform.rotation.eulerAngles;
            rot.x = 90f;
            powerInstance.transform.rotation = Quaternion.Euler(rot);
        }

        if (context.canceled)
        {
            isHeld = false;
            poolManager.ReleaseToPool(powerData.powerVFX, powerInstance);
        }

    }

    protected override bool consumeStamina()
    {
        if (playerData.stamina < powerData.stamina)
        {
            Destroy(powerInstance);
            isHeld = false;
        }

        return base.consumeStamina();
    }

    public void GetHealth()
    {
        Debug.Log("GAINING HEALTH");
        playerData.health += 1;
        playerData.health = Mathf.Clamp(playerData.health, 0, 100);
    }


    public void changeSize(GameObject leechSize)
    {
        leechSize.transform.SetParent(boxAim.transform);
        leechSize.transform.localPosition = new Vector3(0f, 0f, 0.254f);

        float maxDistance = 20f;
        //   Vector3 direction = boxAim.transform.forward;
        Vector3 direction = cam.transform.forward;

        if (Physics.Raycast(cam.transform.position, direction, out RaycastHit hit, maxDistance))
        {
     
            // shrink size
            LayerMask layerMask = 0;
            if (hit.collider == gameObject) return;
            else
            {
                float hitDistance = hit.distance;
                var scale = boxAim.transform.localScale;
                scale.z = Mathf.Clamp(hitDistance, 1f, maxDistance);
                boxAim.transform.localScale = scale;
                Debug.Log(hit.collider);
            }
           
        }
        else
        {
            // Nothing hit: extend normally
            var scale = boxAim.transform.localScale;
            scale.z = Mathf.Clamp(scale.z + maxDistance, 1f, maxDistance);
            boxAim.transform.localScale = scale;
            boxAim.transform.rotation = cam.transform.rotation;
            var rot = boxAim.transform.rotation;
            rot.x = Mathf.Clamp(rot.x, -20f, 20f);
            boxAim.transform.rotation = rot;
         
          
        }
    }




}
