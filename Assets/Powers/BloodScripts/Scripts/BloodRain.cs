using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BloodRain : BasePower
{
  
    private Collider[] enemyColliders;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private GameObject rainPrefab;
    private bool canAttack;
    List<Collider> enemyList = new List<Collider>();

    protected override void Start()
    {
        base.Start();
        canAttack = true;
    }
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


    private IEnumerator ResetAttack(int time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Blood Rain active!");
        canAttack = true;
    }

    private void StartBloodRain()
    {
        Attack();
    }

    protected override void SpawnPower()
    {
        base.SpawnPower();
        RainBlood();
        StartCoroutine(DestroyPower(5, powerInstance));
    }

    private void RainBlood() 
    {
        enemyColliders = Physics.OverlapSphere(transform.position, 100f, enemyLayer);
        if (enemyColliders.Length == 0) return;
        var renderer = powerInstance.GetComponent<Renderer>();
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
        Debug.Log("Spawning blood drop");
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

    private void ResetAnim()
    {
        animator.SetBool("BloodRain", false);
        StartCoroutine(ResetAttack(15));
    }




}



