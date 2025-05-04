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

    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    public Animator animator;
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.Play("show");
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
            
            return;
        }
        DialogueLine curentLine = lines.Dequeue();

        characterIcon.sprite = curentLine.character.icon;
        
    }
}
