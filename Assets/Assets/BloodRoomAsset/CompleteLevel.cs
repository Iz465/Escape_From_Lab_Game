using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (GlobalEnemyManager.levelComplete)
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(levelName);
        }
          
    }
}
