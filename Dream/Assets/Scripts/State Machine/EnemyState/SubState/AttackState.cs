using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class AttackState : AbilityState
    {
        private bool isAttackEnd;

        public AttackState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            obj.animator.Play("Attack0", 0);
            obj.Attack();


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