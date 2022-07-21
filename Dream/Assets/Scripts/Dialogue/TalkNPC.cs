using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNPC : MonoBehaviour
{
    public DialogueData data;

    [SerializeField] private bool canTalk;
    private void Update()
    {
        if (canTalk && InputHandler.instance.Movement.y > 0.5f)
            Talk();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    private void Talk()
    {
        EventManager.instance.TalkToNPCEvent.Invoke(data);
        GameManager.instance.ChangeGameState(GameManager.GameState.GameMenu);
    }
}
