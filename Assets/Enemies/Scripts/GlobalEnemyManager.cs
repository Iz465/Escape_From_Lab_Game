using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static bool enemyAttacking;
    public static HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

    private void Update()
    {
        RandomiseAttack();
    }
    // Only one random enemy in attack range attacks the player at a time.
    public int RandomiseAttack()
    {
        int random = Random.Range(0, enemiesInRange.Count);

        return random;
    }

  
    
}
