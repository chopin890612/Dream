using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;


namespace Bang.StateMachine.PlayerMachine
{
    public class AirState : State<TestPlayer, PlayerData>
    {

        private bool isGrounded;
        public AirState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isGrounded && obj._rb.velocity.y < 0.01f)
                stateMachine.ChangeState(obj.landState);
            else
            {
                obj.XMovement(obj._inputActions.rawMove);
                obj.CheckIfShouldFlip();
            }
            if (obj._inputActions.JumpButton == false && obj.jumpState.holdJump == true)
                obj.jumpState.holdJump = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}