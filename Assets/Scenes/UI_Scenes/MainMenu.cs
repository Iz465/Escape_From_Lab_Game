using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static SaveablePlayer saveFile = null;
    private void Start()
    {
        modes.Add("Exclusive Full Screen", FullScreenMode.ExclusiveFullScreen);
        modes.Add("Full Screen Window",FullScreenMode.FullScreenWindow);
        modes.Add("Windowed",FullScreenMode.Windowed);
        modes.Add("Maximized Window",FullScreenMode.MaximizedWindow);

        difficulties.Add("Easy", Difficulty.Easy);
        difficulties.Add("Normal", Difficulty.Normal);
        difficulties.Add("Challenging", Difficulty.Challenging);
        difficulties.Add("Hard", Difficulty.Hard);
        
        saveFile = new SaveablePlayer();
        string data = PlayerPrefs.GetString("Data","");
        if(data != "")
            saveFile = JsonUtility.FromJson<SaveablePlayer>(data);
        print(data);

        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        {
            GetComponent<Canvas>().enabled = false;
            mainMenu.SetActive(false);
            settingsPage.SetActive(false);
            
            if (data == "") return;
            if (saveFile.characterChosen != null && saveFile.characterChosen != "")
            {
                print("spawn in character");
                Cursor.lockState = CursorLockMode.Locked;
                Transform characterSelection = uisToEnableOnPlay[0].transform.Find("Characters");
                characterSelection.GetComponent<CharacterSelection>().MakeCharacter(saveFile.characterChosen);
                characterSelection.gameObject.SetActive(false);
            }
        }


        difficultyText.text = "Normal";
        if (data == "") return;
        foreach (var (key, val) in difficulties)
        {
            if(val == saveFile.difficulty)
            {
                difficultyText.text = key;
                return;
            }
        }
    }
    public static IEnumerator WaitForLoad()
    {
        while (saveFile == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        

        yield return null;
    }

    public enum Difficulty 
    { 
        Easy,
        Normal,
        Challenging,
        Hard
    }

    public void Back()
    {
        settingsPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    #region main region
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsPage;
    [SerializeField] List<GameObject> uisToEnableOnPlay = new();

    public void Play()
    {
        if(uisToEnableOnPlay.Count > 0)
        {
            for(int i = 0; i < uisToEnableOnPlay.Count; i++)
            {
                uisToEnableOnPlay[i].SetActive(true);
            }
        }

     

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            Debug.Log("SCENE LOADED");
            SceneManager.LoadScene(1);
        }
        else
        {
            GetComponent<Canvas>().enabled = false;
            mainMenu.SetActive(false);
            settingsPage.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

            if(saveFile.characterChosen != "")
            {
                uisToEnableOnPlay[0].GetComponent<CharacterSelection>().MakeCharacter(saveFile.characterChosen);
                uisToEnableOnPlay[0].SetActive(false);
            }

        }

        if (saveFile == null)
            SceneManager.LoadScene(18);


        Time.timeScale = 1;
    }

    public void Settings()
    {
        settingsPage.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Exit()
    {
        //save
        if (saveFile.characterChosen != null)
            saveFile.playedBefore = true;

        print("saving");
        string save = JsonUtility.ToJson(saveFile);
        print(save);
        PlayerPrefs.SetString("Data", save);
        PlayerPrefs.Save();
        print("saved");
        Application.Quit(); 
    }
    #endregion

    #region screenmode
    [SerializeField] Text screenMode;
    Dictionary<string,FullScreenMode> modes = new Dictionary<string, FullScreenMode>();
    public void ScreenRight()
    {
        bool goNext = false;
        foreach(var (key, value) in modes)
        {
            if (goNext)
            {
                screenMode.text = key;
                Screen.fullScreenMode = value;
                return;
            }
            if(key == screenMode.text && !goNext)
            {
                if(key != "Maximized Window")
                {
                    goNext = true;
                }
                else
                {
                    screenMode.text = "Exclusive Full Screen";
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    return;
                }
                
            }
        }
    }

    public void ScreenLeft()
    {
        string prevKey = "";
        FullScreenMode prevValue = FullScreenMode.FullScreenWindow;

        string currentKey = "";
        FullScreenMode currentValue = FullScreenMode.FullScreenWindow;

        foreach (var (key, value) in modes)
        {
            prevKey = currentKey;
            prevValue = currentValue;

            currentValue = value;
            currentKey = key;

            if (key == screenMode.text)
            {
                if (key != "Exclusive Full Screen")
                {
                    screenMode.text = prevKey;
                    Screen.fullScreenMode = prevValue;
                }
                else
                {
                    screenMode.text = "Maximized Window";
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    return;
                }
            }
        }
    }
    #endregion

    #region difficulty

    public static Difficulty difficulty;
    Dictionary<string, Difficulty> difficulties = new Dictionary<string, Difficulty>();
    public Text difficultyText;
    public void NextDifficulty()
    {
        bool goNext = false;
        foreach (var (key, value) in difficulties)
        {
            if (goNext)
            {
                difficulty = value;
                saveFile.difficulty = difficulty;
                difficultyText.text = key;
                return;
            }
            if (key == difficultyText.text && !goNext)
            {
                if (key != "Hard")
                {
                    goNext = true;
                }
                else
                {
                    difficultyText.text = "Easy";
                    difficulty = Difficulty.Easy;
                    saveFile.difficulty = difficulty;
                    return;
                }

            }
        }
    }

    public void PreviousDifficulty()
    {
        string prevKey = "";
        Difficulty prevValue = Difficulty.Easy;

        string currentKey = "";
        Difficulty currentValue = Difficulty.Easy;

        foreach (var (key, value) in difficulties)
        {
            prevKey = currentKey;
            prevValue = currentValue;

            currentValue = value;
            currentKey = key;

            if (key == difficultyText.text)
            {
                if (key != "Easy")
                {
                    difficultyText.text = prevKey;
                    difficulty = prevValue;
                    saveFile.difficulty = difficulty;
                }
                else
                {
                    difficultyText.text = "Hard";
                    difficulty = Difficulty.Hard;
                    saveFile.difficulty = difficulty;
                    return;
                }
            }
        }
    }

    #endregion

    void Pause()
    {
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        GetComponent<Canvas>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
