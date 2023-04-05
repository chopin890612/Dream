using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPCTalk Event", menuName = "Bang's Things/ScriptObjects/Event Data/NPC Talk Event")]
public class NPCTalkDEvent : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.NPCTalkDEvent.Invoke();
    }
}