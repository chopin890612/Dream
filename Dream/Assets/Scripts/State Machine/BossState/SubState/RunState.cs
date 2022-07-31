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

            if(obj.seePlayer == false || obj.playerBeside == true)
            {
                stateMachine.ChangeState(obj.idleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Drag(1);
            obj.Run(1, true, 1);
        }
    }
}