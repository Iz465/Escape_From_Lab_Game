using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    public bool successDoor;
    public bool challengeRoom;

    public string roomToFinish;
    public string roomName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(Load(other));
            //other.transform.parent = null;
        }
    }

    IEnumerator Spawn(GameObject obj)
    {
        Scene current = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.MoveGameObjectToScene(obj, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(current);
    }

    IEnumerator Load(Collider other)
    {
        if(!other.transform.CompareTag("Player")) yield return null;

        SaveablePlayer save = SaveablePlayer.saveFile;
        if (successDoor)
        {
            if (!save.roomsFinished.Contains(roomName))
            {
                save.roomsFinished.Add(roomName);

                if(roomName == "Speed1")
                    SaveablePlayer.saveFile.speedRoom2 = true;
                if (roomName == "Speed2")
                    SaveablePlayer.saveFile.speedRoom3 = true;
            }

        }

        if (challengeRoom)
        {
            if(roomToFinish == "")
            {
                yield return Spawn(other.transform.gameObject);
            }else if (save.roomsFinished.Contains(roomToFinish))
            {
                yield return Spawn(other.transform.gameObject);
            }
        }
        else
        {
            yield return Spawn(other.transform.gameObject);
        }

        
    }
}