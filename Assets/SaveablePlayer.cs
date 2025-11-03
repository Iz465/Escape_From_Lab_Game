using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveablePlayer
{
    public List<string> roomsFinished = new List<string>();
    public MainMenu.Difficulty difficulty;
    public FullScreenMode screenMode;
}
