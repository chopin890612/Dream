using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToState : StateMachineBehaviour
{
    public string toState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(toState)) animator.Play(toState, layerIndex);
    }
}
