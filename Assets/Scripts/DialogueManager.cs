using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines ;
    
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    public Animator animator;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.SetTrigger("Show");
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.lines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            //EndDialogue();
            return;
        }
        DialogueLine curentLine = lines.Dequeue();

        characterIcon.sprite = curentLine.character.icon;
        characterName.text = curentLine.character.name;
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(curentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    //void EndDialogue()
   // {
       // isDialogueActive = false;
       // animator.SetBool("Hide", true);
    //}
}
