using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(Load(other));
            //other.transform.parent = null;
        }
    }

    IEnumerator Load(Collider other)
    {
        Scene current = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.MoveGameObjectToScene(other.transform.gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(current);
    }
}