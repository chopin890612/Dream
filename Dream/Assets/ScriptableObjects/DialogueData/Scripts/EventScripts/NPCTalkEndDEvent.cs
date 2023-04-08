using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Talk End Event", menuName = "Bang's Things/ScriptObjects/Event Data/NPC Talk End Event")]
public class NPCTalkEndDEvent : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.NPCTalkEndDEvent.Invoke();
    }
}