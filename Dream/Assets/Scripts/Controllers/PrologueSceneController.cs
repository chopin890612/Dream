using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSceneController : MonoBehaviour
{
    public TimelineControl controller;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.instance.LoadingCompleteEvent.AddListener(LoadingCompleteHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadingCompleteHandler()
    {
        controller.ChangePlayable(0);
    }
}
