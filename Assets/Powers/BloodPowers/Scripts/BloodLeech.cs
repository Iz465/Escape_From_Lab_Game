using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class BloodLeech : HoldPower, ICollide

{
    BloodPowerData bloodData;
  //  [SerializeField]
 //   private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask playerLayer;
    

    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;
    }

    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        UpdateBeam();
        return base.UseStamina();
    }

    // dynamically changes the length of blood leech based on how far the enemy is.
    void UpdateBeam()
    {
        
        if (!powerInstance) return;
        powerInstance.transform.SetParent(boxAim.transform);
        RaycastHit hit;
        float beamLength = 20f;

        if (Physics.Raycast(boxAim.position, boxAim.forward, out hit, 20f, playerLayer))
        {
            beamLength = hit.distance + 1.5f; 
        }

   
        Vector3 localScale = powerInstance.transform.localScale;
        localScale.z = beamLength;
        powerInstance.transform.localScale = localScale;

   
        powerInstance.transform.localPosition = new Vector3(0, 0, beamLength / 2f);
        powerInstance.transform.rotation = boxAim.rotation;
       
    }

    public void CollideResult(Collider objectHit, GameObject collider)
    {
        Debug.Log($"Health is : {playerData.health}, blood health is : {bloodData.getHealth}");
        playerData.health += bloodData.getHealth;
        playerData.health = Mathf.Clamp(playerData.health, 0, playerData.maxHealth);
    }

}
  