using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class GroundState : State<Enemy, EnemyData>
    {
        public GroundState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
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