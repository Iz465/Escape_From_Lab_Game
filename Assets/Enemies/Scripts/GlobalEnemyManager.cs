using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static bool enemyAttacking;
    public static HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    public static HashSet<GameObject> knightsInRange = new HashSet<GameObject>();
    public static HashSet<GameObject> totalEnemies = new HashSet<GameObject>();
    public static HashSet<GameObject> totalMeleeZombies = new HashSet<GameObject>();
    public static HashSet<GameObject> totalEvilKnights = new HashSet<GameObject>();



    public static bool levelComplete;

    // Handles all enemy code instead of requiring every enemy in the game to have to handle this script. Only one will be in the game world. 
    //

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
        totalEnemies.Remove(enemy);
        enemiesInRange.Remove(enemy);
        if (enemiesInRange.Count <= 0)
        {
          //  Debug.Log("No Enemies Left!");

  
        }

   
    
    }

    // Activates enemy spawns that only happen when no enemies of a certain type are left.
    public void RespawnEnemyWave(HashSet<GameObject> enemyType, GameObject enemy)
    {
        enemyType.Remove(enemy);
        if (enemyType.Count > 0) return;

        if (delayedSpawns.Count == 0) return;

        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject oldSpawn in delayedSpawns)
        {
            SpawnEnemy enemySpawn = oldSpawn.GetComponent<SpawnEnemy>();
            StartCoroutine(enemySpawn.StartSound(1));

            foreach (GameObject spawn in enemySpawn.enemySpawns)
            {
                Instantiate(enemySpawn.spawnParticle, spawn.transform.position, Quaternion.identity);
                StartCoroutine(enemySpawn.SpawnIn(spawn, 1));
            }

            enemySpawn.waveAmount -= 1;
            if (enemySpawn.waveAmount <= 0)
                toRemove.Add(oldSpawn);
        }

        foreach (var oldSpawn in toRemove)
        {
            delayedSpawns.Remove(oldSpawn);
        }
    }





    // Only one random enemy in attack range attacks the player at a time.
    public int RandomiseAttack()
    {
        int random = Random.Range(0, enemiesInRange.Count);

        return random;
    }


    public static bool canMakeSound = true;
    public IEnumerator ResetSound(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("SOUND RESET");
        canMakeSound = true;
    }

  

    int deathSounds = 3, attackSounds = 2, fleshSounds = 5, footstepSounds = 1;

    

    int deathAmount = 0, attackAmount = 0, fleshAmount = 0, footstepAmount = 0;
    public void CheckEnemySound(AudioClip sound,  string soundType, AudioSource audioSouceInstance)
    {
        switch (soundType)
        {
            case "death":  if (deathAmount < deathSounds) StartCoroutine(PlayEnemySound(sound, "death", audioSouceInstance)); break;
            case "attack": if (attackAmount < attackSounds) StartCoroutine(PlayEnemySound(sound, "attack", audioSouceInstance)); break;
            case "flesh": if (fleshAmount < fleshSounds) StartCoroutine(PlayEnemySound(sound, "flesh", audioSouceInstance)); break;
            case "footsteps": if (footstepAmount < footstepSounds) StartCoroutine(PlayEnemySound(sound, "footsteps", audioSouceInstance)); break;
        }
    }

    public IEnumerator PlayEnemySound(AudioClip sound, string soundType, AudioSource audioSouceInstance)
    {
        audioSouceInstance.PlayOneShot(sound);
        
        switch (soundType)
        {
            case "death":  deathAmount++; break;
            case "attack": attackAmount++; break;
            case "flesh": fleshAmount++; break;
            case "footsteps": footstepAmount++; break;
        }

        yield return new WaitForSeconds(sound.length);

        switch (soundType)
        {
            case "death": deathAmount--; break;
            case "attack": attackAmount--; break;
            case "flesh": fleshAmount--; break;
            case "footsteps": footstepAmount--; break;
        }

    }

    [HideInInspector] public static float healthMultiplier = 1;
    [HideInInspector] public static float speedMultiplier = 1;
    [HideInInspector] public static float damageMultiplier = 1;
    [HideInInspector] public static float waveAmount = 3;
    

    public static void UpdateEnemyStats(float multiplierAmount)
    {
        healthMultiplier = multiplierAmount;
        speedMultiplier = multiplierAmount;
        damageMultiplier = multiplierAmount;
    }
}
