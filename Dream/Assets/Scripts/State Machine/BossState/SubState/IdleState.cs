using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class IdleState : OnGroundState
    {
        public IdleState(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.animator.Play("Idle", 0);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.seePlayer == true && obj.playerBeside == false)
            {
                stateMachine.ChangeState(obj.runState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Drag(1);
            obj.Run(1, true, 0);
        }
    }
}