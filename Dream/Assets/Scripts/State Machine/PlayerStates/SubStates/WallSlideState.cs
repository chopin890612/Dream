using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class WallSlideState : OnWallState
    {
        public WallSlideState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.SetGravityScale(0);
            obj.animator.Play("WallClimb", 0);
        }

        public override void ExitState()
        {
            base.ExitState();
            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (objData.enableWallJump && obj.LastPressedJumpTime > 0)
            {
                stateMachine.ChangeState(obj.wallJumpState);
            }
            else if (objData.enableSlide && InputHandler.instance.Movement.y == 0)
            {
                stateMachine.ChangeState(obj.wallIdleState);
            }
            else if ((obj.LastOnWallLeftTime > 0 && InputHandler.instance.Movement.x >= 0) || (obj.LastOnWallRightTime > 0 && InputHandler.instance.Movement.x <= 0))
            {
                stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //obj.Drag(objData.dragAmount);
            //obj.Slide();
            obj.Climb();
        }
    }
}