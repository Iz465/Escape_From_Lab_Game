using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;




[CreateAssetMenu(fileName = "SettingsBindings", menuName = "Scriptable Objects/SettingsBindings")]
public class SettingsBindings : ScriptableObject
{
    [System.Serializable]
    public class DisplaySettings
    {
        public List<string> ResolutionOptions = new() {
        "1280×720",
        "1920×1080",
        "2560×1440"
        };

        [Range(0, 2)]
        public int ResolutionIndex;

        public ToggleButtonGroupState FullscreenToggleState;
        public ToggleButtonGroupState HDRToggleState;
        public ToggleButtonGroupState WindowsBorderToggleState;
    }
    [System.Serializable]
    public class GraphicsSettings
    {
        public ToggleButtonGroupState QualityToggleState;
        public ToggleButtonGroupState ShadowsToggleState;
        public ToggleButtonGroupState LightingToggleState;
        public ToggleButtonGroupState TexturesToggleState;
        public ToggleButtonGroupState AntiAliasingToggleState;
        public ToggleButtonGroupState MotionBlurToggleState;

    }
    [System.Serializable]
    public class ControlSettings
    {
        [Range(0.1f, 10f)]
        public float MouseSensitivity;

        public ToggleButtonGroupState InverVerticalAimToggleState;
        public ToggleButtonGroupState InvertHorizontalAimToggleState;
    }
    [System.Serializable]
    public class AudioSettings
    {
        [Range(-80f, 20f)]
        public float MusicVolume;

        [Range(-80f, 20f)]
        public float SfxVolume;

        [Range(-80f, 20f)]
        public float Dialogue;

        [Range(-80f, 20f)]
        public float Master;

        public ToggleButtonGroupState SubtitlesToggleState;
    }
    public DisplaySettings displaySettings = new DisplaySettings();/* Display settings*/
    public GraphicsSettings graphicsSettings = new GraphicsSettings();/* Graphics settings*/
    public ControlSettings controlSettings = new ControlSettings();/* Control settings*/
    public AudioSettings audioSettings = new AudioSettings();/* Audio settings*/

}
