using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BirdFTalk Event", menuName = "Bang's Things/ScriptObjects/Event Data/BirdF Talk Event")]
public class BirdFirstTalk : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.BirdFirstTalk.Invoke();
    }
}
