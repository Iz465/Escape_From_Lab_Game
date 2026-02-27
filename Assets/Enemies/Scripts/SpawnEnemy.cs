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


    private List<GameObject> enemiesAlive = new List<GameObject>(); // this tracks the enemies alive that the specific spawn made.

    public List<SpawnEnemy> spawns = new List<SpawnEnemy>(); // different enemy spawns that can spawn after this one ends. 

    public List<SpawnEnemy> spawnsBeforeRespawning = new List<SpawnEnemy>();
   
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



        ActivateSpawn();


       Collider collider = gameObject.GetComponent<Collider>(); // spawner can only be triggered once.
       collider.enabled = false;
    }

    public void ActivateSpawn()
    {
        foreach (GameObject spawn in enemySpawns)
        {

            Instantiate(spawnParticle, spawn.transform.position, Quaternion.identity);

            StartCoroutine(SpawnIn(spawn, 1));
        }



        Debug.Log("Player Entered!");
        StartCoroutine(StartSound(1));

    }

    public IEnumerator SpawnIn(GameObject spawn, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject enemyAlive = Instantiate(enemyPrefab, spawn.transform.position, Quaternion.identity);
        navmeshtestscript navmeshtest = enemyAlive.GetComponent<navmeshtestscript>();
        navmeshtest.enemiesSpawner = this;
        enemiesAlive.Add(enemyAlive);

    }

    public void RemoveEnemy(GameObject enemy)
    {
  //      Debug.Log("REMOVING ENEMY FROM SPAWN COUNT");
        enemiesAlive.Remove(enemy);
        if (enemiesAlive.Count == 0)
        {
         //   Debug.Log("All ENEMIES IN THE SPAWN ARE DEAD");

            if (waveAmount > 0) // respawns the enemy wave.
            {
                ActivateSpawn();
                waveAmount -= 1;
            }


            else
            {
                spawnsBeforeRespawning.Remove(this);
                foreach (SpawnEnemy checkSpawwn in spawnsBeforeRespawning)
                {
                    checkSpawwn.spawnsBeforeRespawning.Remove(this);
                }

                if (spawnsBeforeRespawning.Count == 0)
                {
                    foreach (SpawnEnemy spawn in spawns)
                        spawn.ActivateSpawn();
                    Destroy(gameObject);
                }

            
           
            }
     
        }

    }

    public IEnumerator StartSound(float time)
    {
        yield return new WaitForSeconds(time);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(spawnSound);
    }

}
