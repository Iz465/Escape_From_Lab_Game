using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BloodThrow : BasePower, ICollide
{
    BloodPowerData bloodData;
    private float radius = 100f;
    [SerializeField]
    private LayerMask enemyLayer;
    private Collider[] enemyDetected;
    private void Awake()
    {
        bloodData = (BloodPowerData)powerData;

    }

    // this ability will explode when collides
    protected override bool UseStamina()
    {
        if (!bloodData) return false;
        playerData.health -= bloodData.loseHealth;
        playerData.health = Mathf.Clamp(playerData.health, 10, playerData.maxHealth);


        return base.UseStamina();
    }

    public void CollideResult(GameObject power)
    {
        enemyDetected = Physics.OverlapSphere(power.transform.position, radius, enemyLayer);
        if (enemyDetected.Length <= 0) return;
        StartCoroutine(TrackEnemy(3f, power));




    }

    private IEnumerator TrackEnemy(float time, GameObject power)
    {
        float timer = 0;
        var startLoc = power.transform.position;
   //     var enemyLoc = enemyDetected[0].transform.position;
       
        Debug.Log("Tracking enemy");
        while (timer < time)
        {
            power.transform.position = Vector3.Lerp(startLoc, enemyDetected[0].transform.position, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }

    }


}



