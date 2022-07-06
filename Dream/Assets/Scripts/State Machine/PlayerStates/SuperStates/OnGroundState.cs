using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnGroundState : State<TestPlayer, PlayerData>
    {
        public OnGroundState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            obj.dashState.ResetDashes();
            obj.jumpState.ResetJumps();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.LastPressedDashTime > 0 && obj.dashState.CanDash())
            {
                obj.stateMachine.ChangeState(obj.dashState);
            }
            else if (obj.LastPressedJumpTime > 0 && obj.jumpState.CanJump())
            {
                stateMachine.ChangeState(obj.jumpState);
            }
            else if(obj.LastOnGroundTime <= 0)
            {
                obj.jumpState.Jumped();
                stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.frictionAmount);
        }
    }
}