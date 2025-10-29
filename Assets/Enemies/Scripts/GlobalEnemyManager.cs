using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static bool enemyAttacking;
    public static HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    public static HashSet<GameObject> totalEnemies = new HashSet<GameObject>();
    public static HashSet<GameObject> totalMeleeZombies = new HashSet<GameObject>();


    public static bool levelComplete;

        private void Start()
    {
        levelComplete = false;
        //navmeshtestscript.canAttack = true;
        enemiesInRange.Clear();
        totalEnemies.Clear();

      //  StartCoroutine(ResetEveryFiveSeconds(3));
    }

   

    public void AddEnemy(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
    }



    public static List<GameObject> delayedSpawns = new List<GameObject>();
    public void EmptyEnemies(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        if (enemiesInRange.Count <= 0)
        {
            Debug.Log("No Enemies Left!");

         

           
            
        }

   
    
    }

    // Activates enemy spawns that only happen when no enemies of a certain type are left.
    public void EmptyMeleeZombies(GameObject zombie)
    {
        totalMeleeZombies.Remove(zombie);
        if (totalMeleeZombies.Count > 0) return;


        if (delayedSpawns.Count == 0) return;
        
        foreach (GameObject oldSpawn in delayedSpawns)
        {
            SpawnEnemy enemySpawn = oldSpawn.GetComponent<SpawnEnemy>();

            foreach (GameObject spawn in enemySpawn.enemySpawns)
            {
                Instantiate(enemySpawn.spawnParticle, spawn.transform.position, Quaternion.identity);
                StartCoroutine(enemySpawn.SpawnIn(spawn, 1));

            }
            enemySpawn.waveAmount -= 1;

            if (enemySpawn.waveAmount <= 0)
                delayedSpawns.Remove(oldSpawn);

        }

        

        
    }




    // Only one random enemy in attack range attacks the player at a time.
    public int RandomiseAttack()
    {
        int random = Random.Range(0, enemiesInRange.Count);

        return random;
    }





}
