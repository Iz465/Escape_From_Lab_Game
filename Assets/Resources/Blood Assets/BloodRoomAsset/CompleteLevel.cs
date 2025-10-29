using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (GlobalEnemyManager.totalEnemies.Count == 0) GlobalEnemyManager.levelComplete = true; 
        Player player = other.GetComponent<Player>();
        if (!player) return;
        if (GlobalEnemyManager.levelComplete)
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(levelName);
        }
          
    }
}
