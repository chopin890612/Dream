﻿using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnWallState : State<TestPlayer, PlayerData>
    {
        public OnWallState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
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

            if (obj.LastPressedDashTime > 0 && obj.dashState.CanDash())
            {
                obj.stateMachine.ChangeState(obj.dashState);
            }
            else if (obj.LastOnGroundTime > 0)
            {
                obj.stateMachine.ChangeState(obj.idleState);
            }
            else if (obj.LastOnWallTime <= 0)
            {
                obj.stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}