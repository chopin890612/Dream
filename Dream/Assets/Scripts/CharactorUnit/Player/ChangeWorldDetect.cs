using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorldDetect : MonoBehaviour
{
    public bool isChangeWorld = false;

    private void Start()
    {
        EventManager.instance.EndChangeWorldEvent.AddListener(EndChangeWorld);
    }
    private void EndChangeWorld()
    {
        isChangeWorld = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isChangeWorld = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isChangeWorld = false;
    }
}
