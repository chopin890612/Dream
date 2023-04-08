using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public UnityEvent PlayerDeadEvent = new UnityEvent();
    public UnityEvent EndChangeWorldEvent = new UnityEvent();

    public UnityEvent FireEvent = new UnityEvent();

    public UnityEvent<string> LevelChangeEvent = new UnityEvent<string>();

    public UnityEvent DialogueEndEvent = new UnityEvent();
    public UnityEvent<DialogueData> TalkToNPCEvent = new UnityEvent<DialogueData>();

    public UnityEvent BirdPlatformEvent = new UnityEvent();

    public UnityEvent LoadingCompleteEvent = new UnityEvent();

    public UnityEvent NPCTalkDEvent = new UnityEvent();
    public UnityEvent NPCTalkEndDEvent = new UnityEvent();

    public UnityEvent DeerPillarEvent = new UnityEvent();

    public UnityEvent BirdFirstTalk = new UnityEvent();
    public UnityEvent BirdTalkEnd = new UnityEvent();
    public UnityEvent BirdTalkEvent = new UnityEvent();

    public UnityEvent<string> GetRelicEvent = new UnityEvent<string>();
    private void Awake()
    {
        instance = this;
    }
}
