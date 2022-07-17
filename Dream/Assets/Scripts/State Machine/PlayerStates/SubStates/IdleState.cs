using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class IdleState : OnGroundState
    {
        public IdleState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.skeletonAnimation.AnimationState.SetAnimation(0, obj.idle, true);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(InputHandler.instance.Movement.x != 0)
            {
                stateMachine.ChangeState(obj.runState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Run(1, true);
        }
    }
}