using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BloodRain : BasePower
{
  
    private Collider[] enemyColliders;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private GameObject rainPrefab;
    private bool canAttack = true;
    private List<Collider> enemyList = new List<Collider>();



    // This function has a bool used to make sure this attack cant be spammed
    public override void StartAttack(InputAction.CallbackContext context) 
    {
        if (!context.performed) return;


        if (canAttack)
        {
            canAttack = false;
            Debug.Log($"starting attack!!!");
            base.StartAttack(context);
        }
        else if (!canAttack)
        {
            Debug.Log($"Unable to attack!");
        }

    }

    // Attack can only be used again once THIS ienumerator has finished 
    private IEnumerator ResetAttack(int time)
    {
        Player.abilityCooldown = 0;

        while (Player.abilityCooldown < time)
        {
            Player.abilityCooldown += Time.deltaTime;
            yield return null;
        }
        Debug.Log("BlOOD RAIN RESET");
        Player.abilityCooldown = 35;
        canAttack = true;

    }




    // Animation event 
    private void StartBloodRain()
    {
        Attack();
    }
    
    // power lasts for five seconds
    protected override void SpawnPower()
    {
        base.SpawnPower();
        RainBlood();
        StartCoroutine(DestroyPower(5, powerInstance));
    }

    // blood power rains down on random enemies caught in the radius 
    private void RainBlood() 
    {
        enemyColliders = Physics.OverlapSphere(transform.position, 100f, enemyLayer);
        if (enemyColliders.Length == 0) return;
        var renderer = powerInstance.GetComponent<Renderer>();
        if (!renderer) return;
        enemyList.Clear();
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

    // power spawns from a random position
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

    
    private IEnumerator ShootRain(float time, GameObject rain)
    {

        yield return new WaitForSeconds(time);
        Debug.Log("Spawning blood drop");
        Collider target = null;
        Collider collider = rain.GetComponent<Collider>();
        if (!collider) yield break;
        if (enemyList.Count == 0)
        {
            Debug.Log("BLOOD RAIN ENEMY LIST IS EMPTY");
            poolManager.ReleaseToPool(rain);
            yield break;
        }

        target = enemyList[0];
        enemyList.RemoveAt(0);
      
        if (!target)
        {
            Debug.Log("BLOOD RAIN TARGET IS NULL");
            poolManager.ReleaseToPool(rain);
            yield break;
        }

        Vector3 direction = (target.transform.position - collider.transform.position).normalized;
        rain.SetActive(true);
        Rigidbody rainBody = rain.GetComponent<Rigidbody>();
        rainBody.AddForce(direction * stats.speed, ForceMode.Impulse);
        enemyList.Add(target);
      
    }

    private void ResetAnim()
    {
        StartCoroutine(ResetAttack(35));
    }




}



