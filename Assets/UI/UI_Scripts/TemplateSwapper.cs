using UnityEngine;
using UnityEngine.UIElements;

public class TemplateSwapper : MonoBehaviour
{
    // References to the UI templates
    public VisualTreeAsset templateA;
    public VisualTreeAsset templateB;

    private VisualElement _root;
    private TemplateContainer _current;
    private Button _swapBtn;

    void OnEnable()
    {
        // Grab root from UIDocument
        var uiDoc = GetComponent<UIDocument>();
        _root = uiDoc.rootVisualElement;

        // Query the button and the placeholder container
        _swapBtn = _root.Q<Button>("swapButton");
        var host = _root.Q<VisualElement>("templateRoot");

        // Instantiate and add the first template
        _current = templateA.Instantiate();
        _current.name = templateA.name;
        host.Add(_current);

        // Wire up the swap logic
        _swapBtn.clicked += () =>
        {
            // Remove the currently displayed template
            _current.RemoveFromHierarchy();

            // Choose the other template
            if(_current.name == templateA.name)
            {
                _current = null;
                _current  = templateB.Instantiate();
                _current.name = templateB.name;
            }
            else
            {
                _current = null;
                _current = templateA.Instantiate();
                _current.name = templateA.name;
            }
                
            
               
            
           

            // Add it back to the same host
            host.Add(_current);
        };
    }

}
