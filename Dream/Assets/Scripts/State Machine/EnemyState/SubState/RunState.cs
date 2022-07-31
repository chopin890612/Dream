using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class RunState : GroundState
    {
        public RunState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
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

            if (Time.time - startTime > 10f)
                stateMachine.ChangeState(obj.idleState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Run(1, true, 1f);
        }
    }
}