using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BloodRain : SpawnPower
{
  
    private Collider[] enemyColliders;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private GameObject rainPrefab;

    List<Collider> enemyList = new List<Collider>();




    protected override void FirePower(GameObject power)
    {
        base.FirePower(power);
        RainBlood(power);
        StartCoroutine(DestroyPower(5, power));   
    }


  //  protected override bool UseStamina()
//    { 
    //    player.stats.health -= bloodData.loseHealth;
  //      player.stats.health = Mathf.Clamp(player.stats.health, 10, player.maxHealth);
    //    return base.UseStamina();
 //   }

    private void RainBlood(GameObject power) 
    {
        enemyColliders = Physics.OverlapSphere(transform.position, 100f, enemyLayer);
        if (enemyColliders.Length == 0) return;
        var renderer = power.GetComponent<Renderer>();
        if (!renderer) return;

        foreach (var enemy in enemyColliders)
            enemyList.Add(enemy);
    
     
        for (int i = 0; i < 10; i++)
        {
      
            Vector3 randomPos = SpawnPos(renderer);
            GameObject test = poolManager.SpawnFromPool(rainPrefab, randomPos, transform.rotation);
            if(!test) return;
      
          
            test.SetActive(false);
            StartCoroutine(ShootRain(i * 0.5f, test));
     
        }
    }

    
    private Vector3 SpawnPos(Renderer renderer)
    {
        Bounds bounds = renderer.bounds;
        Vector3 center = bounds.center;

        float boundsRadius = Mathf.Min(bounds.extents.x, bounds.extents.z);
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * boundsRadius;
        float x = center.x + distance * Mathf.Cos(angle);
        float z = center.z + distance * Mathf.Sin(angle);
        float y = bounds.min.y;

        return new Vector3(x, y, z);
    }

    private IEnumerator ShootRain(float time, GameObject test)
    {

        yield return new WaitForSeconds(time);
        Collider target = null;
        Collider collider = test.GetComponent<Collider>();
        if (!collider) yield break;
        if (enemyList.Count == 0)
        {
            poolManager.ReleaseToPool(test);
            yield break;
        }

        target = enemyList[0];
        enemyList.RemoveAt(0);
      
        if (!target)
        {
            poolManager.ReleaseToPool(test);
            yield break;
        }

        Vector3 direction = (target.transform.position - collider.transform.position).normalized;
        test.SetActive(true);
        Rigidbody rainBody = test.GetComponent<Rigidbody>();
        rainBody.AddForce(direction * stats.speed, ForceMode.Impulse);
        enemyList.Add(target);
      
    }




}



