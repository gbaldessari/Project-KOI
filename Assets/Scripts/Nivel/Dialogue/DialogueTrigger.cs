using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;

    public bool TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues);
        return true;
    }
}
