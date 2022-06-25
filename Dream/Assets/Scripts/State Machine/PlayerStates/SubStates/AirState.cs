using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;


namespace Bang.StateMachine.PlayerMachine
{
    public class AirState : State<TestPlayer, PlayerData>
    {
        private bool isGrounded;
        private bool isOnWall;
        private bool jumpButton;
        private bool isJumping;
        private bool canNextJump;
        public AirState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isGrounded = obj.CheckOnGround();
            isOnWall = obj.CheckOnWall();
            jumpButton = obj._inputActions.JumpButton;
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

            jumpButton = obj._inputActions.JumpButton;
            CheckJumpMultiplier();
            if (isGrounded && obj._rb.velocity.y < 0.01f)
            {
                stateMachine.ChangeState(obj.landState);
            }
            //Multiple Jump
            else if (jumpButton && obj.jumpState.CanJump() && obj._inputActions.isJumped == false)
            {
                stateMachine.ChangeState(obj.jumpState);
            }
            else
            {
                obj.XMovement(obj._inputActions.rawMove);
                obj.CheckIfShouldFlip();
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void CheckJumpMultiplier()
        {
            if (isJumping)
            {
                if (obj._inputActions.isJumped == false)
                {
                    //Debug.Log(obj._rb.velocity);
                    obj.EndJump();
                    //obj._rb.velocity = new Vector2(obj._rb.velocity.x, objData.jumpSpeed * objData.minJumpMutiplyer);
                    isJumping = false;
                }
                else if (obj._rb.velocity.y <= 0f)
                {
                    isJumping = false;
                }

            }
        }

        public void SetIsJumping() => isJumping = true;
    }
}