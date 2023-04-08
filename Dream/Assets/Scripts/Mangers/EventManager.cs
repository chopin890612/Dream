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

    private void Awake()
    {
        instance = this;
    }
}
