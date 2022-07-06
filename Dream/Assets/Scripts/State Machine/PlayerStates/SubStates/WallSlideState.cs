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
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.LastPressedJumpTime > 0)
            {
                obj.stateMachine.ChangeState(obj.wallJumpState);
            }
            else if ((obj.LastOnWallLeftTime > 0 && InputHandler.instance.Movement.x >= 0) || (obj.LastOnWallRightTime > 0 && InputHandler.instance.Movement.x <= 0))
            {
                obj.stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.dragAmount);
            obj.Slide();
        }
    }
}