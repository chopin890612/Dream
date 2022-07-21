using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class JumpState : AbilityState
    {
        private int jumpsLeft;
        public JumpState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }
        public override void EnterState()
        {
            base.EnterState();

            Jumped();
            obj.Jump();
            obj.skeletonAnimation.AnimationState.SetAnimation(0, obj.jump, false);
            obj.animator.Play("Jump", 0);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.LastAttackTime > 0 && obj.AttackCooldown < 0)
            {
                stateMachine.ChangeState(obj.attackState);
            }
            else if (obj.LastPressedDashTime > 0 && obj.dashState.CanDash())
            {
                stateMachine.ChangeState(obj.dashState);
            }
            else if (obj.LastPressedJumpTime > 0 && obj.LastOnWallTime > 0)
            {
                stateMachine.ChangeState(obj.wallJumpState);
            }
            else if (obj._rb.velocity.y < 0) //Jump performed, change state
            {
                stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.dragAmount);
            obj.Run(1, false);
        }

        public bool CanJumpCut()
        {
            if (obj._rb.velocity.y > 0 && stateMachine.currentState != obj.attackState)//if the player is jumping and still moving up
            {  
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanJump()
        {
            if (jumpsLeft > 0)
                return true;
            else
                return false;
        }
        public void ResetJumps()
        {
            jumpsLeft = objData.jumpAmount;
        }
        public void Jumped()
        {
            jumpsLeft--;
        }
    }
}