using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class BloodLeech : BasePower, ICollide

{
  
  //  [SerializeField]
 //   private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask playerLayer;

    private void Update()
    {
        if (isHeld)
            UpdateBeam();
        
        else if (!isHeld && powerInstance)
        {
            poolManager.ReleaseToPool(powerInstance);
        }
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
            Debug.Log("Leech is hitting object");
            beamLength = hit.distance; 
        }

   
        Vector3 localScale = powerInstance.transform.localScale;
        localScale.z = beamLength;
        powerInstance.transform.localScale = localScale;

   
        powerInstance.transform.localPosition = new Vector3(0, 0, beamLength / 2f);
        powerInstance.transform.rotation = boxAim.rotation;
       
    }

    public void CollideResult(Collider objectHit, GameObject collider)
    {
      //  Debug.Log($"Health is : {player.stats.health}, blood health is : {bloodData.getHealth}");
    //    player.stats.health += bloodData.getHealth;
      //  player.stats.health = Mathf.Clamp(player.stats.health, 0, player.maxHealth);
    }

}
  