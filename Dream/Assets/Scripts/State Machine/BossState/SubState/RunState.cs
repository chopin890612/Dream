using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.BossState
{

    public class RunState : OnGroundState
    {
        public RunState(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.animator.Play("Run", 0);
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
            obj.Run(1, true, 1);
        }
    }
}