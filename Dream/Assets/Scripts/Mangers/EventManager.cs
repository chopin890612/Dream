using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;
    public UnityEvent<bool> SwitchShapeEvent = new UnityEvent<bool>();
    public UnityEvent PlayerDeadEvent = new UnityEvent();

    private void Awake()
    {
        eventManager = this;
    }
}
