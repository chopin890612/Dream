using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bird Talk Event", menuName = "Bang's Things/ScriptObjects/Event Data/Bird Talk Event")]

public class BirdTalkEvent : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.BirdTalkEvent.Invoke();
    }
}
