using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class AbilityState : State<Enemy, EnemyData>
    {
        public AbilityState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
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

            if(obj.knockBackTime > 0)
            {
                stateMachine.ChangeState(obj.knockBackState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}