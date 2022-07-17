﻿using System.Collections;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class AttackState : AbilityState
    {
        private bool isAttackEnd;
        public AttackState(TestPlayer player, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            obj.Attack();
            obj.skeletonAnimation.AnimationState.SetAnimation(0, obj.attack, false);
            isAttackEnd = false;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(isAttackEnd)
            {
                if (obj.LastOnGroundTime > 0)
                {
                    stateMachine.ChangeState(obj.idleState);
                }
                else if(obj.LastOnGroundTime < 0)
                {
                    stateMachine.ChangeState(obj.onAirState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            obj.Drag(objData.attackDragAmount);
        }

        public void IsAttackEnd()
        {
            isAttackEnd = true;
        }
    }
}