using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class DashAttack : AbilityState
    {
        private bool isAttackEnd;
        public DashAttack(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.DashAttack();
            obj.animator.Play("Attack1", 0);

            isAttackEnd = false;
        }

        public override void ExitState()
        {
            base.ExitState();
            obj.animator.Play("Default", 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isAttackEnd)
            {
                if (obj.seePlayer)
                {
                    stateMachine.ChangeState(obj.chaseState);
                }
                else
                {
                    stateMachine.ChangeState(obj.idleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public void EndAttack()
        {
            isAttackEnd = true;
        }
    }
}