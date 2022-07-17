using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class KnockBackState : State<TestPlayer, PlayerData>
    {
        private bool isKnockBacking;
        public KnockBackState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            isKnockBacking = true;
            obj.KnockBack(1);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(obj.LastOnGroundTime > 0 && CanControl())
            {
                stateMachine.ChangeState(obj.idleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.dragAmount);
        }
        public void EndKnockBacking()
        {
            isKnockBacking = false;
        }
        public bool CanControl()
        {
            return obj.LastKnockBackTime < 0;
        }
    }
}