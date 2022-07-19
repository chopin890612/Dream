using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class ChaseState : AbilityState
    {
        public ChaseState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
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
            if (obj.playerInAttackRange && obj.attackTime < 0)
            {
                stateMachine.ChangeState(obj.attackState);
            }
            else if (!obj.seePlayer)
            {
                stateMachine.ChangeState(obj.idleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Chase(1.5f);
        }
    }
}