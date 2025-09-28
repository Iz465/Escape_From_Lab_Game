using System.Collections.Generic;
using System.IO;
using UnityEngine;




[CreateAssetMenu(fileName = "Sample_Data_Hud", menuName = "Scriptable Objects/Sample_Data_Hud")]
public class Sample_Data_Hud : ScriptableObject
{
    [Range(0,100)]
    public int Health;
    [Range(0,100)]
    public int Ammo;


}
