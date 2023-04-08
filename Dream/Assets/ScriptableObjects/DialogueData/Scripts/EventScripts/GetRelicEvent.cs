using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GetRelic Event", menuName = "Bang's Things/ScriptObjects/Event Data/GetRelic Event")]

public class GetRelicEvent : DialogueEventData
{
    public string RelicName;

    public override void EventAction()
    {
        EventManager.instance.GetRelicEvent.Invoke(RelicName);
    }

}
