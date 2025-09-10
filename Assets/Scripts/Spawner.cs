using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
    private void OnEnable()
    {
        print("Current scene:");
        print(SceneManager.GetActiveScene().path);
    }
}
