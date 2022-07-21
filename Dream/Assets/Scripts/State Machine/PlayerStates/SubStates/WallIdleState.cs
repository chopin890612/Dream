using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.PlayerMachine
{
    public class WallIdleState : OnWallState
    {
        public WallIdleState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.SetGravityScale(0);
            obj.animator.Play("WallIdle", 0);
        }

        public override void ExitState()
        {
            base.ExitState();
            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.LastPressedJumpTime > 0)
            {
                stateMachine.ChangeState(obj.wallJumpState);
            }
            else if (InputHandler.instance.Movement.y != 0)
            {
                stateMachine.ChangeState(obj.wallSlideState);
            }
            else if ((obj.LastOnWallLeftTime > 0 && InputHandler.instance.Movement.x >= 0) || (obj.LastOnWallRightTime > 0 && InputHandler.instance.Movement.x <= 0))
            {
                stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Climb();
        }
    }
}