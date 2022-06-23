using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class IdleState : OnGroundState
    {
        public IdleState(TestPlayer obj, StateMachine<TestPlayer, ObjectData> stateMachine, ObjectData objData) : base(obj, stateMachine, objData)
        {

        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("IdleState");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isWalking)
                stateMachine.ChangeState(obj.moveState);
            //Debug.Log(moveDirection);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}