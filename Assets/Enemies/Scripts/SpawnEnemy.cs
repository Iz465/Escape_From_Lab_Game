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
    public bool spawnAfter;
    public int waveAmount = 0;
    [SerializeField] public AudioClip spawnSound;
    private void OnTriggerEnter(Collider other)
    {
        
        Player player = other.GetComponent<Player>();
    
        if (!player) return;

        Collider box = gameObject.GetComponent<Collider>();
        box.enabled = false;


        if (spawnAfter)
        {
            GlobalEnemyManager.delayedSpawns.Add(gameObject);
            return;
        }
        


        foreach (GameObject spawn in enemySpawns)
        {
           
            Instantiate(spawnParticle, spawn.transform.position, Quaternion.identity);
    //        Debug.Log($"Duration: {spawnParticle.duration}");
            StartCoroutine(SpawnIn(spawn, 1));
        }
            

        
        Debug.Log("Player Entered!");
        StartCoroutine(StartSound(1));
      
        Destroy(gameObject, 2f);
    }

    public IEnumerator SpawnIn(GameObject spawn, float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(enemyPrefab, spawn.transform.position, Quaternion.identity);

    }

    public IEnumerator StartSound(float time)
    {
        yield return new WaitForSeconds(time);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(spawnSound);
    }

}
