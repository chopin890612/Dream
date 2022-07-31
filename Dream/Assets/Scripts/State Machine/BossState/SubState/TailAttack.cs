using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class TailAttack : AbilityState
    {
        private bool isAttackEnd;
        public TailAttack(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.TailAttack();
            obj.animator.Play("TailAttack", 0);
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
                if (isAttackEnd)
                {
                    if (obj.seePlayer && obj.playerBeside)
                    {
                        stateMachine.ChangeState(obj.idleState);
                    }
                    else if (obj.seePlayer && !obj.playerBeside)
                    {
                        stateMachine.ChangeState(obj.runState);
                    }
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