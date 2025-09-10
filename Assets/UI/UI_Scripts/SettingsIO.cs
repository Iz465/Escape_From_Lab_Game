using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.Audio;


public static class SettingsIO 
{
    private static string SettingsFilePath => Path.Combine(Application.persistentDataPath, "settings.json");
    public static void SaveSettings(SettingsBindings settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(SettingsFilePath, json);
        Debug.Log($"Settings saved to {SettingsFilePath}");
    }

    public static void LoadSettings(SettingsBindings settings)
    {
        if (File.Exists(SettingsFilePath))
        {
            string json = File.ReadAllText(SettingsFilePath);
            JsonUtility.FromJsonOverwrite(json, settings);
            Debug.Log($"Settings loaded from {SettingsFilePath}");
        }
        else
        {
            Debug.LogWarning($"Settings file not found at{SettingsFilePath} use defaults");
        }
    }
}   
