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
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate(); 

            if(obj.LastPressedJumpTime > 0)
            {
                stateMachine.ChangeState(obj.jumpState);
            }
            else if(obj.LastOnGroundTime <= 0)
            {
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