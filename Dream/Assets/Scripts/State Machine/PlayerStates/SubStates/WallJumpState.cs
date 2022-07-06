using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class WallJumpState : OnWallState
    {
        private int jumpDir;
        public WallJumpState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            jumpDir = obj.LastOnWallRightTime > 0 ? -1 : 1;
            obj.WallJump(jumpDir);
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
            else if (obj.LastOnGroundTime > 0) //Jump performed, change state
            {
                obj.stateMachine.ChangeState(obj.idleState);
            }
            else if (obj.LastPressedJumpTime > 0 && ((obj.LastOnWallRightTime > 0 && jumpDir == 1) || (obj.LastOnWallLeftTime > 0 && jumpDir == -1)))
            {
                obj.stateMachine.ChangeState(obj.wallJumpState);
            }
            else if (Time.time - startTime > objData.wallJumpTime) //Jump performed, change state
            {
                obj.stateMachine.ChangeState(obj.onAirState);
            }

            if ((InputHandler.instance.Movement.x != 0))
                obj.CheckDirectionToFace(InputHandler.instance.Movement.x > 0);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}