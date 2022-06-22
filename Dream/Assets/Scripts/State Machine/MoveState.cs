using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class MoveState : OnGroundState
    {
        public MoveState(TestPlayer obj, StateMachine<TestPlayer, ObjectData> stateMachine, ObjectData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}