using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnGroundState : State<TestPlayer, PlayerData>
    {
        protected bool isWalking;
        protected float moveDirection;
        private bool jumpButton;
        protected bool isOnWall;

        public OnGroundState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isOnWall = obj.CheckOnWall();
        }

        public override void EnterState()
        {
            base.EnterState();
            obj.jumpState.ResetJumpAmout();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            moveDirection = obj._inputActions.rawMove;
            if (moveDirection != 0f)
                isWalking = true;
            else
                isWalking = false;

            jumpButton = obj._inputActions.JumpButton;

            //Change State
            if (jumpButton == true && obj._inputActions.isJumped == false)
                stateMachine.ChangeState(obj.jumpState);            
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}