using UnityEngine;

public class BloodLeech : HoldPower

{
    BloodPowerData bloodData;
    RaycastHit raycastHit;
    Vector3 startloc;
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;
        startloc = boxAim.position;


    }

    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        playerData.health += bloodData.getHealth;
        playerData.health = Mathf.Clamp(playerData.health, 0, playerData.maxHealth);
        ChangeSize();

        return base.UseStamina();
    }

    private void ChangeSize()
    {
        /*
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, 20f)) 
        {
            float distance = raycastHit.distance; // this is the distance from ray start to hit point

            var scale = transform.localScale;
            scale.z = distance; // set local Z scale to match distance
            transform.localScale = scale;
            Debug.Log("HIT"); 
        } */
    }
}
    /*
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

    } */