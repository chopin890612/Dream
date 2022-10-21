using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnAirState : State<TestPlayer, PlayerData>
    {
        public OnAirState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }
        public override void EnterState()
        {
            base.EnterState();
            obj.animator.Play("Fall", 0);
        }

        public override void ExitState()
        {
            base.ExitState();

            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (obj.LastAttackTime > 0 && obj.AttackCooldown < 0)
            {
                stateMachine.ChangeState(obj.attackState);
            }
            else if (obj.LastPressedDashTime > 0 && obj.dashState.CanDash() && obj.statues.Have_Relic_Scale)
            {
                stateMachine.ChangeState(obj.dashState);
            }
            else if (obj.LastPressedJumpTime > 0 && obj.jumpState.CanJump())
            {
                stateMachine.ChangeState(obj.jumpState);
            }            
            else if (obj.LastPressedJumpTime > 0 && obj.LastOnWallTime > 0)
            {
                stateMachine.ChangeState(obj.wallJumpState);
            }
            else if ((obj.LastOnWallLeftTime > 0 && InputHandler.instance.Movement.x < 0) || (obj.LastOnWallRightTime > 0 && InputHandler.instance.Movement.x > 0))
            {
                stateMachine.ChangeState(obj.wallIdleState);
            }
            else if (obj.LastOnGroundTime > 0)
            {
                stateMachine.ChangeState(obj.idleState);                
            }
            else if (obj._rb.velocity.y < 0)
            {
                //quick fall when holding down: feels responsive, adds some bonus depth with very little added complexity and great for speedrunners :D (In games such as Celeste and Katana ZERO)
                if (InputHandler.instance.Movement.y < 0)
                {
                    obj.SetGravityScale(objData.quickFallGravityMult);
                }
                else
                {
                    obj.SetGravityScale(objData.fallGravityMult);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.dragAmount);
            obj.Run(1, false);
        }
    }
}