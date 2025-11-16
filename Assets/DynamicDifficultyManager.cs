using System.Collections;
using UnityEngine;

public class DynamicDifficultyManager : MonoBehaviour
{
    public static float difficultyPoints = 0;
   
    public static int deaths = 0;
    public static float time = 0;
    public static float damage = 0;
    

    public void UpdateTime()
    {
        time += Time.deltaTime;

    }

 

    // The difficulty changes based on how the player performed in the level.
    private void CalculateDifficulty()
    {

        // Too easy for player
        if (difficultyPoints < 25)
        {
            GlobalEnemyManager.UpdateEnemyStats(1.2f, 1.2f);
        }

        // Slightly Too easy for player
        if (difficultyPoints < 50)
        {
            GlobalEnemyManager.UpdateEnemyStats(1.1f, 1.1f);
        }

        // Perfect difficulty
        else if (difficultyPoints > 50 && difficultyPoints < 100)
        {
            GlobalEnemyManager.UpdateEnemyStats(1, 1);
        }

        // Slightly too hard for player
        else if (difficultyPoints > 100)
        {
            GlobalEnemyManager.UpdateEnemyStats(0.9f, 0.9f);
        }

        // Too hard for player 
        else if (difficultyPoints > 150)
            GlobalEnemyManager.UpdateEnemyStats(0.8f, 0.8f);
        
        difficultyPoints = 0;
    }
    

  

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) return;
        CalculateDifficulty();
    }

}
