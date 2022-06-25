using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageState : StateMachineBehaviour
{
    public string onEnter, onExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (!string.IsNullOrEmpty(onEnter)) animator.gameObject.GetComponent<IStateMachine>().StateCallback(onEnter);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(onExit)) animator.gameObject.GetComponent<IStateMachine>().StateCallback(onExit);
    }
}
