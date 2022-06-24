using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;


namespace Bang.StateMachine.PlayerMachine
{
    public class AbilityState : State<TestPlayer, PlayerData>
    {
        protected bool isAbilityDone;
        private bool isGrounded;
        public AbilityState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isGrounded = obj.CheckOnGround();
        }

        public override void EnterState()
        {
            base.EnterState();
            isAbilityDone = false;
        }

        public override void ExitState()
        {
            base.ExitState();
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAbilityDone)
            {
                if (isGrounded && obj._rb.velocity.y < 0.01f)
                {
                    stateMachine.ChangeState(obj.idleState);
                    Debug.Log("I'm Call Idle");
                }
                else
                    stateMachine.ChangeState(obj.airState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}