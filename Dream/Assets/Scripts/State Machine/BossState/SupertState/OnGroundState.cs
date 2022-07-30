using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class OnGroundState : State<Boss, EnemyData>
    {
        public OnGroundState(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}