using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class IdleState : GroundState
    {
        public IdleState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
            
        }
    }
}