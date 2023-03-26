using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNPC : MonoBehaviour
{
    [SerializeField] bool isFirstTalk = true;
    public DialogueData firstTalk;
    public DialogueData[] talk;
    private int talkIndex = 0;

    [SerializeField] bool canTalk;
    private void Update()
    {
        if (isFirstTalk && canTalk && InputHandler.instance.Movement.y > 0.5f)
        {
            Talk(firstTalk);
            isFirstTalk = false;
        }
        else if (canTalk && InputHandler.instance.Movement.y > 0.5f)
        {
            Talk(talk[talkIndex]);
            talkIndex++;
            if (talkIndex >= talk.Length) talkIndex = 0;
        }
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

    private void Talk(DialogueData talkContent)
    {
        EventManager.instance.TalkToNPCEvent.Invoke(talkContent);
        GameManager.instance.ChangeGameState(GameManager.GameState.Dialogue);
    }
}
