using UnityEngine;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class FallToDeath : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (!player)
        {
            player = other.transform.root.gameObject.AddComponent<Player>();
            if (!player) return;
        }
        Debug.Log("Hit void");
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        Debug.Log($"Scene is : {scene.name}");
        SceneManager.LoadScene(scene.name);
    }
}
