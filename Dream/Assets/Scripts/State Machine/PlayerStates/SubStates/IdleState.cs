using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class IdleState : OnGroundState
    {
        public IdleState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            if(obj.enableSpine)
                obj.skeletonAnimation.AnimationState.SetAnimation(0, obj.idle, true);
            if(obj.enableAnimator)
                obj.animator.Play("idle", 0);

            if (obj.CanSlope)
                obj.GetComponent<Collider>().material = obj.infFrction;
            else
                obj.GetComponent<Collider>().material = obj.noFriction;
        }

        public override void ExitState()
        {
            base.ExitState();
            obj.GetComponent<Collider>().material = obj.noFriction;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(InputHandler.instance.Movement.x != 0 && obj.CanSlope && stateMachine.currentState == obj.idleState)
            {
                stateMachine.ChangeState(obj.runState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            obj.Run(1, true);
            //obj.SlopeRun(1);
        }
    }
}