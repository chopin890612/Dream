using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class KnockBackState : State<Enemy, EnemyData>
    {
        public KnockBackState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.KnockBack(1);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(obj.onGround && CanControl())
            {
                stateMachine.ChangeState(obj.idleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public bool CanControl()
        {
            return obj.knockBackTime < 0;
        }
    }
}