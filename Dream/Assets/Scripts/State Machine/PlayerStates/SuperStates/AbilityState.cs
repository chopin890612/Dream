using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;


namespace Bang.StateMachine.PlayerMachine
{
    public class AbilityState : State<TestPlayer, PlayerData>
    {
        public AbilityState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}