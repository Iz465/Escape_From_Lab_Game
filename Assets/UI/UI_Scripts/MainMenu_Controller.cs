using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;


public class MainMenu_Controller : MonoBehaviour
{
    [Header("Settings Document")]
    public UIDocument SettingsMenu_UIDoc;
    [Header("Settings Scriptable Object ")]
    public SettingsBindings settingsBindings;
    [Header("Audio")]
    public AudioMixer mainAudioMixer;

    private VisualElement _root;
    private UIDocument uiDoc;

    private Button SMReturnButton;
    private Button StartPlayButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiDoc.enabled = true;
        Debug.Log("Settings_Controller: Start - Loading Settings");
        SettingsIO.LoadSettings(settingsBindings);
        ApplyVolumeSettings();
    }
    private void ApplyVolumeSettings()
    {
        var audioSettings = settingsBindings.audioSettings;
        static float LinearToDecibel(float linear)
        {
            return Mathf.Log10(linear) * 20;
        }
        mainAudioMixer.SetFloat("Volume_Music", LinearToDecibel(audioSettings.MusicVolume));
        mainAudioMixer.SetFloat("Volume_SFX", LinearToDecibel(audioSettings.SfxVolume));
        mainAudioMixer.SetFloat("Volume_Dialogue", LinearToDecibel(audioSettings.Dialogue));
        mainAudioMixer.SetFloat("Volume_Master", LinearToDecibel(audioSettings.Master));
    }
    private void OnEnable()
    {
        // Grab root from UIDocument
        uiDoc = GetComponent<UIDocument>();
        _root = uiDoc.rootVisualElement;
        
        // Query the button and the placeholder container
        StartPlayButton = _root.Q<Button>("PlayButton");
        SMReturnButton = _root.Q<Button>("SMReturnButton");

        StartPlayButton.clickable.clicked += () =>
        {
            Debug.Log("Play Clicked");
            UnityEngine.SceneManagement.SceneManager.LoadScene("HUD");
        };

        SMReturnButton.clickable.clicked += () =>
        {
            Debug.Log("Swap Clicked");
            uiDoc.rootVisualElement.PlaceBehind(SettingsMenu_UIDoc.rootVisualElement);
        };
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
