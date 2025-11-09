using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] private string levelName;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();

        director.stopped += StartLevel;
    }

    private void StartLevel(PlayableDirector directorInstance)
    {
        SceneManager.LoadScene(levelName);
    }

}
