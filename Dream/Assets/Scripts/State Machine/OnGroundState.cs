using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnGroundState : State<TestPlayer, ObjectData>
    {
        public OnGroundState(TestPlayer obj, StateMachine<TestPlayer, ObjectData> stateMachine, ObjectData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}