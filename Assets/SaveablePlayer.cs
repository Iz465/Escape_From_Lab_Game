using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveablePlayer
{
    public List<string> roomsFinished = new List<string>();
    public MainMenu.Difficulty difficulty;
    public FullScreenMode screenMode;

    public bool playedBefore;

    public string characterChosen;
    public bool superRun;
    public bool slowTime;
    public bool punch;
    public bool phaze;
    public bool dash;

    public bool speedRoom1;
    public bool speedRoom2;
    public bool speedRoom3;
    public bool iceRoom1;
    public bool iceRoom2;
    public bool iceRoom3;
}
