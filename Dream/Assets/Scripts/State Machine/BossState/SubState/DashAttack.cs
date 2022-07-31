using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class DashAttack : AbilityState
    {
        private bool isAttackEnd;
        public DashAttack(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.DashAttack();
            obj.animator.Play("DashAttack", 0);
            isAttackEnd = false;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAttackEnd)
            {
                if(obj.seePlayer && obj.playerBeside)
                {
                    stateMachine.ChangeState(obj.idleState);
                }
                else if(obj.seePlayer && !obj.playerBeside)
                {
                    stateMachine.ChangeState(obj.runState);
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