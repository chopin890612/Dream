using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class AttackState : AbilityState
    {
        private bool isAttacking;

        public AttackState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            isAttacking = true;
            obj.Attack();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isAttacking)
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
            isAttacking = false;
        }
    }
}