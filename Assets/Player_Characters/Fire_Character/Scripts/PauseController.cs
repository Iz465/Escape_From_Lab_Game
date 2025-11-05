using UnityEngine;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    private bool _isPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isPaused = !_isPaused;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Time.timeScale = 0.0f; // Pause the game
                GamePaused.Invoke();
            }
            else
            {
                Time.timeScale = 1.0f; // Resume the game
                GameResumed.Invoke();
            }
        }
    }
}
