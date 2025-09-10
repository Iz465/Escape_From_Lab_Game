using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;




[CreateAssetMenu(fileName = "SettingsBindings", menuName = "Scriptable Objects/SettingsBindings")]
public class SettingsBindings : ScriptableObject
{
    [System.Serializable]
    public class DisplaySettings
    {

        [Tooltip("List of resolution options in the format 'width x height'")]
        public List<string> ResolutionOptions = new() {
        "1280×720",
        "1920×1080",
        "2560×1440"
        };

        [DefaultValue(1)]
        [Range(0, 2)]
        public int ResolutionIndex;

        [DefaultValue(0)]
        public ToggleButtonGroupState FullscreenToggleState;

        [DefaultValue(1)]
        public ToggleButtonGroupState HDRToggleState;

        [DefaultValue(1)]
        public ToggleButtonGroupState WindowsBorderToggleState;
    }
    [System.Serializable]
    public class GraphicsSettings
    {
        [DefaultValue(2)]
        public ToggleButtonGroupState QualityToggleState;

        [DefaultValue(1)]
        public ToggleButtonGroupState ShadowsToggleState;

        [DefaultValue(2)]
        public ToggleButtonGroupState LightingToggleState;

        [DefaultValue(2)]
        public ToggleButtonGroupState TexturesToggleState;

        [DefaultValue(2)]
        public ToggleButtonGroupState AntiAliasingToggleState;

        [DefaultValue(0)]
        public ToggleButtonGroupState MotionBlurToggleState;

    }
    [System.Serializable]
    public class ControlSettings
    {
        [Range(0.1f, 10f)]
        public float MouseSensitivity;

        [DefaultValue(0)]
        public ToggleButtonGroupState InverVerticalAimToggleState;

        [DefaultValue(0)]
        public ToggleButtonGroupState InvertHorizontalAimToggleState;
    }
    [System.Serializable]
    public class AudioSettings
    {
        [DefaultValue(0.5f)]
        [Range(0.0001f, 1f)]
        public float MusicVolume;

        [DefaultValue(0.5f)]
        [Range(0.0001f, 1f)]
        public float SfxVolume;

        [DefaultValue(0.5f)]
        [Range(0.0001f, 1f)]
        public float Dialogue;

        [DefaultValue(0.5f)]
        [Range(0.0001f, 1f)]
        public float Master;

        [DefaultValue(1)]
        public ToggleButtonGroupState SubtitlesToggleState;
    }
    public DisplaySettings displaySettings = new DisplaySettings();/* Display settings*/
    public GraphicsSettings graphicsSettings = new GraphicsSettings();/* Graphics settings*/
    public ControlSettings controlSettings = new ControlSettings();/* Control settings*/
    public AudioSettings audioSettings = new AudioSettings();/* Audio settings*/

}
