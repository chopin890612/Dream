using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.DialogueEndEvent.AddListener(SetDisable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
