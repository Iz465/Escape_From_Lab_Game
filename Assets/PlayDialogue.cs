using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    [SerializeField] private int dialogueIndex;
    [SerializeField] private DialogueScriptableSystem dialogueScriptableObject;
    [SerializeField] private DialogueManager dialogueManager;
 

    private void OnTriggerEnter(Collider other)
    {
      
    }


    public void DialogueTriggered()
    {
        Debug.Log("PLAYING");
        dialogueManager.StartCoroutine(dialogueManager.ShowDialogueSlowly(dialogueScriptableObject.dialogue[dialogueIndex], dialogueManager.typingSpeed));
        Destroy(gameObject);
    }
}
