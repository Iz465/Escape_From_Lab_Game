using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObjectScript", menuName = "Scriptable Objects/DialogueScriptableObject")]
public class DialogueScriptableSystem : ScriptableObject
{
    [TextArea]
    public string[] dialogue;
}
