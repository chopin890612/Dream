using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Collider Collider;
    public int nextSceneID;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<BoxCollider>();
        EventManager.instance.DialogueEndEvent.AddListener(SetTeleportActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTeleportActive()
    {
        Collider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.LoadScene(nextSceneID);
        }
    }
}
