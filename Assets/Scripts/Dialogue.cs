using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Diyalog Ayarlarý")]
    public TextMeshProUGUI textComponent;
    [SerializeField] private float textSpeed;
    public GameObject dialoguePanel;
    
    [Header("Diyalog Ýçerikleri")]
    public string[] internalMonologue; // Karakterin iç sesi
    public string[] npcDialogueLines; // NPC diyaloglarý
    
    [Header("Görsel Ýpucu")]
    public GameObject pressEHint;
    public Vector3 hintOffset = new Vector3(0, 1f, 0);

    private int index;
    private bool isDialogueActive = false;
    private bool isPlayerInRange = false;
    private Canvas canvas;
    private bool hasPlayedMonologue = false; // Ýç monolog oynatýldý mý?
    private bool isInternalMonologue = false; // Þu an iç monolog mu?

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null) canvas.enabled = false;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        textComponent.text = string.Empty;
        
        if (pressEHint != null)
        {
            pressEHint.SetActive(false);
            pressEHint.transform.position = transform.position + hintOffset;
        }

        // Oyun baþýnda iç monologu baþlat
        if (!hasPlayedMonologue && internalMonologue.Length > 0)
        {
            StartInternalMonologue();
        }
    }

    void Update()
    {
        if (isInternalMonologue)
        {
            // Ýç monolog sýrasýnda hýzlý geçme
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (textComponent.text == internalMonologue[index])
                {
                    NextMonologueLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = internalMonologue[index];
                }
            }
        }
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                if (textComponent.text == npcDialogueLines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = npcDialogueLines[index];
                }
            }
        }
        
        if (pressEHint != null && pressEHint.activeSelf)
        {
            pressEHint.transform.position = transform.position + hintOffset;
        }
    }

    void StartInternalMonologue()
    {
        isInternalMonologue = true;
        isDialogueActive = true;
        hasPlayedMonologue = true;
        
        if (canvas != null) canvas.enabled = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeMonologueLine());
    }

    IEnumerator TypeMonologueLine()
    {
        foreach (char c in internalMonologue[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextMonologueLine()
    {
        if (index < internalMonologue.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeMonologueLine());
        }
        else
        {
            EndInternalMonologue();
        }
    }

    void EndInternalMonologue()
    {
        isInternalMonologue = false;
        isDialogueActive = false;
        
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (canvas != null) canvas.enabled = false;
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        
        if (canvas != null) canvas.enabled = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (pressEHint != null) pressEHint.SetActive(false);
        
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeDialogueLine());
    }

    IEnumerator TypeDialogueLine()
    {
        foreach (char c in npcDialogueLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < npcDialogueLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeDialogueLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (canvas != null) canvas.enabled = false;

        if (isPlayerInRange && pressEHint != null)
        {
            if (canvas != null) canvas.enabled = true;
            pressEHint.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            if (canvas != null && !isDialogueActive) canvas.enabled = true;
            if (!isDialogueActive && pressEHint != null) pressEHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (pressEHint != null) pressEHint.SetActive(false);
            if (isDialogueActive && !isInternalMonologue) EndDialogue();
            if (canvas != null && !isInternalMonologue) canvas.enabled = false;
        }
    }
}