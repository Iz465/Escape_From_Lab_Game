using System.Collections;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Entities.EntitiesJournaling;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    [SerializeField] private DialogueScriptableSystem dialogueObject;
    

    private bool doOnce = true;


    private void Update()
    {
        if (doOnce)
        {
            doOnce = false; 
            StartCoroutine(SwitchDialogue(5));
        }
        
 
    }

    private IEnumerator SwitchDialogue(float time)
    {
        int number = -1;
        while (true)
        {
            yield return new WaitForSeconds(time);
            number++;
            switch (number)
            {
                case 0: dialogueText.text = dialogueObject.dialogue[0]; Debug.Log(dialogueObject.dialogue[0]); break;
                case 1: dialogueText.text = dialogueObject.dialogue[1]; break;
                case 2: dialogueText.text = dialogueObject.dialogue[2]; break;
                case 3: dialogueText.text = dialogueObject.dialogue[3]; number = -1; break;
            }
        }
    }






}
