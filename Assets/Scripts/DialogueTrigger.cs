using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)] 
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}