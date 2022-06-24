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
        public OnGroundState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
            moveDirection = obj._inputActions.rawMove;
            if (moveDirection != 0f)
                isWalking = true;
            else
                isWalking = false;

            jumpButton = obj._inputActions.JumpButton;

            if(jumpButton == false && obj.jumpState.holdJump == true)
                obj.jumpState.holdJump = false;

            if (jumpButton == true && obj.jumpState.holdJump == false)
                stateMachine.ChangeState(obj.jumpState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public void ResetIsJumped()
        {

        }
    }
}