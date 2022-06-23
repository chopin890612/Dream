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

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("MoveState");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!isWalking)
                stateMachine.ChangeState(obj.idleState);

            obj._rb.velocity = new Vector2(moveDirection * objData.GetData("moveSpeed").GetValue<float>(), obj._rb.velocity.y);
            obj.CheckIfShouldFlip();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}