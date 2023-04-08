using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pillar Event", menuName = "Bang's Things/ScriptObjects/Event Data/Pillar Event")]
public class LiltPillarEvent : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.DeerPillarEvent.Invoke();
    }
}