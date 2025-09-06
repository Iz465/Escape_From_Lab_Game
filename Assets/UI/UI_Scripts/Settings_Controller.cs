using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;
using Unity.VisualScripting;

public class Settings_Controller : MonoBehaviour
{
    public VisualTreeAsset DefaultTemplate;
    public VisualTreeAsset Template_Settings_Display;
    public VisualTreeAsset Template_Settings_Graphics;
    public VisualTreeAsset Template_Settings_Controls;
    public VisualTreeAsset Template_Settings_Audio;

    private VisualElement _root;
    private TemplateContainer _current;
    private Button DisplayButton;
    private Button GraphicsButton;
    private Button ControlsButton;
    private Button AudioButton;
    private UIDocument uiDoc;

    public SettingsBindings settingsBindings;


    void Start()
    {
        Debug.Log("Settings_Controller: Start - Loading Settings");
        SettingsIO.LoadSettings(settingsBindings);
    }

    private void OnEnable()
    {
        // Grab root from UIDocument
        uiDoc = GetComponent<UIDocument>();
        _root = uiDoc.rootVisualElement;

        // Query the button and the placeholder container
        DisplayButton = _root.Q<Button>("DisplayButton");
        GraphicsButton = _root.Q<Button>("GraphicsButton");
        ControlsButton = _root.Q<Button>("ControlsButton");
        AudioButton = _root.Q<Button>("AudioButton");
        var host = _root.Q<VisualElement>("templateRoot");

        // Instantiate and add the first template
        _current = DefaultTemplate.Instantiate();
        _current.name = DefaultTemplate.name;
        host.Add(_current);
        void Settings_page_buttons(char ButtonName) 
        {
            _current.RemoveFromHierarchy();
            _current = null;
            switch (ButtonName)
            {
                case 'D':
                    DisplayButton.SetEnabled(false);
                    GraphicsButton.SetEnabled(true);
                    ControlsButton.SetEnabled(true);
                    AudioButton.SetEnabled(true);
                    _current = Template_Settings_Display.Instantiate();
                    _current.name = Template_Settings_Display.name;
                    break;
                case 'G':
                    DisplayButton.SetEnabled(true);
                    GraphicsButton.SetEnabled(false);
                    ControlsButton.SetEnabled(true);
                    AudioButton.SetEnabled(true);
                    _current = Template_Settings_Graphics.Instantiate();
                    _current.name = Template_Settings_Graphics.name;
                    break;
                case 'C':
                    DisplayButton.SetEnabled(true);
                    GraphicsButton.SetEnabled(true);
                    ControlsButton.SetEnabled(false);
                    AudioButton.SetEnabled(true);
                    _current = Template_Settings_Controls.Instantiate();
                    _current.name = Template_Settings_Controls.name;
                    break;
                case 'A':
                    DisplayButton.SetEnabled(true);
                    GraphicsButton.SetEnabled(true);
                    ControlsButton.SetEnabled(true);
                    AudioButton.SetEnabled(false);
                    _current = Template_Settings_Audio.Instantiate();
                    _current.name = Template_Settings_Audio.name;
                    break;

            }
            host.Add(_current);
        }

        DisplayButton.clicked += () => Settings_page_buttons('D');
        GraphicsButton.clicked += () => Settings_page_buttons('G');
        ControlsButton.clicked += () => Settings_page_buttons('C');
        AudioButton.clicked += () => Settings_page_buttons('A');
    }

    private void OnDisable()
    {
        
        SettingsIO.SaveSettings(settingsBindings); // Use the instance to call SaveSettings
      
    }
}
