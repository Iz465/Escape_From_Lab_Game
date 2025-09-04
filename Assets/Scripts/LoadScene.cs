using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string scenePath;
    [SerializeField] LoadSceneParameters parameters;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.MoveGameObjectToScene(other.transform.gameObject, SceneManager.GetSceneByBuildIndex(1));
            SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
            //other.transform.parent = null;
        }
    }
}
