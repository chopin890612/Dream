using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bird TalkEnd Event", menuName = "Bang's Things/ScriptObjects/Event Data/Bird Talk EndEvent")]
public class BirdTalkEndEvnet : DialogueEventData
{
    public override void EventAction()
    {
        EventManager.instance.BirdTalkEnd.Invoke();
    }
}
