using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class JumpState : AbilityState
    {
        public JumpState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }
        public override void EnterState()
        {
            base.EnterState();

            obj.Jump();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj._rb.velocity.y <= 0) //Jump performed, change state
            {
                obj.stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.dragAmount);
            obj.Run(1);
        }

        public bool CanJumpCut()
        {
            if(obj.stateMachine.currentState == this && obj._rb.velocity.y > 0) //if the player is jumping and still moving up
                return true;
            else
                return false;
        }
    }
}