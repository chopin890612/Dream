using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class DashState : AbilityState
    {
        private Vector2 dir;
        private int dashesLeft;

        private bool dashAttacking;
        public DashState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            dashesLeft--;

            dir = Vector2.zero; //get direction to dash in
            if (InputHandler.instance.Movement == Vector2.zero)
                dir.x = (obj.IsFacingRight) ? 1 : -1;
            else
                dir = InputHandler.instance.Movement;

            dashAttacking = true;
            obj.Dash(dir);
        }

        public override void ExitState()
        {
            base.ExitState();

            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Time.time - startTime > objData.dashAttackTime + objData.dashEndTime) //dashTime over transition to another state
            {
                if (obj.LastOnGroundTime > 0)
                    obj.stateMachine.ChangeState(obj.idleState);
                else
                    obj.stateMachine.ChangeState(obj.onAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (Time.time - startTime > objData.dashAttackTime)
            {
                //initial dash phase over, now begin slowing down and giving control back to player
                obj.Drag(objData.dragAmount);
                obj.Run(objData.dashEndRunLerp); //able to apply some run force but will be limited (~50% of normal)

                if (dashAttacking)
                    StopDash();
            }
            else
            {
                obj.Drag(objData.dashAttackDragAmount);
            }
        }

        private void StopDash()
        {
            dashAttacking = false;
            obj.SetGravityScale(objData.gravityScale);

            if (dir.y > 0)
            {
                if (dir.x == 0)
                    obj._rb.AddForce(Vector2.down * obj._rb.velocity.y * (1 - objData.dashUpEndMult), ForceMode.Impulse);
                else
                    obj._rb.AddForce(Vector2.down * obj._rb.velocity.y * (1 - objData.dashUpEndMult) * .7f, ForceMode.Impulse);
            }
        }

        public bool CanDash()
        {
            return dashesLeft > 0;
        }

        public void ResetDashes()
        {
            dashesLeft = objData.dashAmount;
        }
    }
}