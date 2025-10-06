using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static bool enemyAttacking;
    public static HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    public static HashSet<GameObject> totalEnemies = new HashSet<GameObject>();
    public static bool levelComplete;

        private void Start()
    {
        levelComplete = false;
        navmeshtestscript.canAttack = true;
        enemiesInRange.Clear();
        totalEnemies.Clear();
    }

    private void Update()
    {
        RandomiseAttack();
    }

    public void AddEnemy(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
    }

    public void EmptyEnemies(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
        if (enemiesInRange.Count <= 0)
        {
            Debug.Log("No Enemies Left!");
            levelComplete = true;
        }
    
    }

    // Only one random enemy in attack range attacks the player at a time.
    public int RandomiseAttack()
    {
        int random = Random.Range(0, enemiesInRange.Count);

        return random;
    }

   



}
