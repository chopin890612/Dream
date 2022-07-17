using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{

    public class RunState : OnGroundState
    {
        public RunState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.skeletonAnimation.AnimationState.SetAnimation(0, obj.walk, true);
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Run(1, true);
        }
    }
}