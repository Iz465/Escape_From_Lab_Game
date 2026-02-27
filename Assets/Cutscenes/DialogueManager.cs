using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueParent;
    [SerializeField] public float typingSpeed;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioSource audioSource;




    public IEnumerator ShowDialogueSlowly(string sentence ,float wordSpeed)
    {
        Debug.Log("SHOW DIALOGUE");
        dialogueParent.SetActive(true);
        foreach (Transform child in dialogueParent.GetComponentInChildren<Transform>(true))
            child.gameObject.SetActive(true);

        dialogueText.enabled = true;
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            if (audioSource) audioSource.PlayOneShot(typingSound, 0.5f);
            yield return new WaitForSeconds(wordSpeed);
        }

        StartCoroutine(DisableDialogue(2));

    }

    private IEnumerator DisableDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (Transform child in dialogueParent.GetComponentInChildren<Transform>(true))
            child.gameObject.SetActive(false);
    }






}


/*
  private IEnumerator SwitchDialogue(float time)
  {

      int number = -1;
      while (true)
      {
          yield return new WaitForSeconds(time);
          number++;
          switch (number)
          {
              case 0: StartCoroutine(ShowDialogueSlowly(dialogueObject.dialogue[0], typingSpeed)); break;
              case 1: StartCoroutine(ShowDialogueSlowly(dialogueObject.dialogue[1], typingSpeed)); break;
              case 2: StartCoroutine(ShowDialogueSlowly(dialogueObject.dialogue[2], typingSpeed)); break;
              case 3: StartCoroutine(ShowDialogueSlowly(dialogueObject.dialogue[3], typingSpeed)); break;
          }
          time = switchSpeed;
      }


  } 
*/