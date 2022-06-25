using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class JumpState : AbilityState
    {
        private int jumpAmount;

        public JumpState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
            jumpAmount = objData.jumpAmount;
        }

        public override void EnterState()
        {
            base.EnterState();
            jumpAmount--;
            obj.airState.SetIsJumping();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Jump(objData.jumpSpeed);
            isAbilityDone = true;
        }
        public bool CanJump()
        {
            if (jumpAmount > 0)
            {
                return true;
            }
            else
                return false;
        }
        public void ResetJumpAmout() => jumpAmount = objData.jumpAmount;
    }
}