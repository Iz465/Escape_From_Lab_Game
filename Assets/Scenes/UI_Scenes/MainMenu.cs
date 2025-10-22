using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
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
    }

    public enum Difficulty 
    { 
        Easy,
        Normal,
        Challenging,
        Hard
    }

    #region main region
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsPage;
    public void Play() => SceneManager.LoadScene(1);

    public void Settings()
    {
        settingsPage.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Exit() => Application.Quit();
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

    #region difficultu

    Dictionary<string, Difficulty> difficulties = new Dictionary<string, Difficulty>();

    public Difficulty difficulty;
    public void NextDifficulty()
    {
        bool goNext = false;
        foreach (var (key, value) in difficulties)
        {
            if (goNext)
            {
                difficulty = value;
                screenMode.text = key;
                return;
            }
            if (key == screenMode.text && !goNext)
            {
                if (key != "Hard")
                {
                    goNext = true;
                }
                else
                {
                    screenMode.text = "Easy";
                    difficulty = Difficulty.Easy;
                    return;
                }

            }
        }
    }

    public void PreviousDifficulty()
    {
        
    }

    #endregion
}
