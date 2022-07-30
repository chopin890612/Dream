using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.PlayerMachine
{
    public class LandState : OnGroundState
    {
        public LandState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.animator.Play("Land", 0);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(InputHandler.instance.Movement.x == 0)
            {
                stateMachine.ChangeState(obj.idleState);
            }
            else if (InputHandler.instance.Movement.x != 0 && obj.CanSlope)
            {
                stateMachine.ChangeState(obj.runState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}