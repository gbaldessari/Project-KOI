using UnityEngine;

public class DialoguesContainer : MonoBehaviour
{
    public DialogueTrigger beforeLevel;
    public DialogueTrigger beforeBoss;
    public DialogueTrigger afterBoss;

    [HideInInspector] public bool beforeLevelTriggered = false;
    [HideInInspector] public bool beforeBossTriggered = false;
    [HideInInspector] public bool afterBossTriggered = false;

    public void TriggerDialogue()
    {
        if (!beforeLevelTriggered) beforeLevelTriggered = beforeLevel.TriggerDialogue();
        else if (!beforeBossTriggered) beforeBossTriggered = beforeBoss.TriggerDialogue();
        else if (!afterBossTriggered) afterBossTriggered = afterBoss.TriggerDialogue();
    }
}
