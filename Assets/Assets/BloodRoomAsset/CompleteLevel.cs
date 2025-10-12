using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) return;
        if (GlobalEnemyManager.levelComplete)
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(levelName);
        }
          
    }
}
