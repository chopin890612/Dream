using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;

namespace Bang.StateMachine.PlayerMachine
{
    public class OnGroundState : State<TestPlayer, ObjectData>
    {
        protected bool onGround;
        protected bool isWalking;
        protected float moveDirection;
        public OnGroundState(TestPlayer obj, StateMachine<TestPlayer, ObjectData> stateMachine, ObjectData objData) : base(obj, stateMachine, objData)
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
            moveDirection = obj.inputActions.rawMove;
            if (moveDirection != 0f)
                isWalking = true;
            else
                isWalking = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            onGround =  Physics.SphereCast(obj.transform.position + new Vector3(0, objData.GetData("anchorOffset").GetValue<float>(), 0), objData.GetData("groundDetectRadius").GetValue<float>(), Vector3.down, 
                out RaycastHit hitGround, objData.GetData("groundDetectDistance").GetValue<float>(), LayerMask.GetMask("Ground"));

            obj._rb.velocity = new Vector2(moveDirection * objData.GetData("moveSpeed").GetValue<float>(), obj._rb.velocity.y);
        }
    }
}