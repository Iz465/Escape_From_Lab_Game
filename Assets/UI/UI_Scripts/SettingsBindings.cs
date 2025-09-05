using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SettingsBindings", menuName = "Scriptable Objects/SettingsBindings")]
public class SettingsBindings : ScriptableObject
{
    // Display settings
    public List<string> ResolutionOptions = new List<string>{ "1280×720", "1920×1080", "2560×1440" };
    public int ResolutionIndex;
    public ToggleButtonGroupState FullscreenToggleState;
    public ToggleButtonGroupState HDRToggleState;
    public ToggleButtonGroupState WindowsBorderToggleState;
    // Graphics Settings
    public ToggleButtonGroupState QualityToggleState;
    public ToggleButtonGroupState ShadowsToggleState;
    public ToggleButtonGroupState LightingToggleState; 
    public ToggleButtonGroupState TexturesToggleState;
    public ToggleButtonGroupState AntiAliasingToggleState;
    public ToggleButtonGroupState MotionBlurToggleState;
    //Controls Settings
    public float MouseSensitivity;
    public ToggleButtonGroupState InverVerticalAimToggleState;
    public ToggleButtonGroupState InvertHorizontalAimToggleState;
    //AudioSettings
    public float MusicVolume;
    public float SfxVolume;
    public float Dialogue;
    public float Master;
    public ToggleButtonGroupState SubtitlesToggleState;



}
