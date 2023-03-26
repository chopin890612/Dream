using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bird Event", menuName = "Bang's Things/ScriptObjects/Event Data/Bird Event")]
public class BirdEvent : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.BirdPlatformEvent.Invoke();
    }
}
