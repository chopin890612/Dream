using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class OnGroundState : State<Boss, EnemyData>
    {
        public OnGroundState(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
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

            if(obj.attackTime < 0 && obj.playerInAttackRange)
            {
                stateMachine.ChangeState(obj.tailAttack);
            }
            else if(obj.abilityCooldown < 0 && obj.attackTime < 0 && obj.playerInFarRange)
            {
                stateMachine.ChangeState(obj.dashAttack);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.ChangeFace();
        }
    }
}