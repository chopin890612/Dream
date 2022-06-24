using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class MoveState : OnGroundState
    {
        public MoveState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
            if (!isWalking)
                stateMachine.ChangeState(obj.idleState);            
            obj.CheckIfShouldFlip();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.XMovement(obj._inputActions.rawMove);
        }
    }
}