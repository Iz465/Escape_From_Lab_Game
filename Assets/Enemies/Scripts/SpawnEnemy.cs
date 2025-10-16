using NUnit.Framework;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class SpawnEnemy : MonoBehaviour
{
    public List<GameObject> enemySpawns = new List<GameObject>();
    public GameObject enemyPrefab;
    public ParticleSystem spawnParticle;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
    
        if (!player) return;

        foreach (GameObject spawn in enemySpawns)
        {
           
            Instantiate(spawnParticle, spawn.transform.position, Quaternion.identity);
    //        Debug.Log($"Duration: {spawnParticle.duration}");
            StartCoroutine(SpawnIn(spawn, 1));
        }
            
        
        Debug.Log("Player Entered!");

        Collider box = gameObject.GetComponent<Collider>();
     

        Destroy(gameObject, 2f);
    }

    private IEnumerator SpawnIn(GameObject spawn, float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(enemyPrefab, spawn.transform.position, Quaternion.identity);

    }

}
