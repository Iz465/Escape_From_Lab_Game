using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    [SerializeField] private int dialogueIndex;
    [SerializeField] private DialogueScriptableSystem dialogueScriptableObject;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int dialogueAmount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PLAYING");
        dialogueManager.StartCoroutine(dialogueManager.ShowDialogueSlowly(dialogueScriptableObject.dialogue[dialogueIndex], dialogueManager.typingSpeed, dialogueAmount));
        Destroy(gameObject);
    }

}
