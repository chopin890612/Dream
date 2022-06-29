using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;
    public UnityEvent<bool> SwitchShapeEvent = new UnityEvent<bool>();

    private void Awake()
    {
        eventManager = this;
    }
}
