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
        }

        public override void ExitState()
        {
            base.ExitState();

            obj.SetGravityScale(objData.gravityScale);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(obj.LastOnGroundTime > 0)
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
            obj.Run(1);
        }
    }
}