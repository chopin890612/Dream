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
            if (obj.CanSlope)
            {
                obj.dashState.ResetDashes();
                obj.jumpState.ResetJumps();
            }
            obj.SetGravityScale(10);
        }

        public override void ExitState()
        {
            base.ExitState();
            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(obj.LastAttackTime > 0 && obj.AttackCooldown < 0)
            {
                stateMachine.ChangeState(obj.attackState);
            }
            else if (obj.LastPressedDashTime > 0 && obj.dashState.CanDash())
            {
                stateMachine.ChangeState(obj.dashState);
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
            else if (((obj.LastOnWallLeftTime > 0 && InputHandler.instance.Movement.x < 0) || (obj.LastOnWallRightTime > 0 && InputHandler.instance.Movement.x > 0)) && InputHandler.instance.Movement.y != 0)
            {
                stateMachine.ChangeState(obj.wallIdleState);
            }
            //else if (!obj.CanSlope)
            //{
            //    obj.SetGravityScale(5);
            //}
            //else
            //{
            //    obj.SetGravityScale(objData.gravityScale);
            //}
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.frictionAmount);
        }
    }
}