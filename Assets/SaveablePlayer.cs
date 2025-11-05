using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveablePlayer
{
    public static SaveablePlayer saveFile;
    public List<string> roomsFinished = new List<string>();
    public MainMenu.Difficulty difficulty;
    public FullScreenMode screenMode;

    public bool playedBefore;
}
