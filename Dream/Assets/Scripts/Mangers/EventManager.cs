using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public UnityEvent<bool> SwitchShapeEvent = new UnityEvent<bool>();
    public UnityEvent PlayerDeadEvent = new UnityEvent();


    public UnityEvent DialogueEndEvent = new UnityEvent();
    public UnityEvent<DialogueData> TalkToNPCEvent = new UnityEvent<DialogueData>();

    private void Awake()
    {
        instance = this;
    }
}
