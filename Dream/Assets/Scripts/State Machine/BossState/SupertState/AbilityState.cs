using System.Collections;
using UnityEngine;

namespace Bang.StateMachine.BossState
{
    public class AbilityState : State<Boss, EnemyData>
    {
        public AbilityState(Boss obj, StateMachine<Boss, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}