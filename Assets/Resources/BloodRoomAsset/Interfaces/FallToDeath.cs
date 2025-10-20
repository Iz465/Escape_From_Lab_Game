using UnityEngine;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class FallToDeath : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit void");
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        Debug.Log($"Scene is : {scene.name}");
        SceneManager.LoadScene(scene.name);
    }
}
