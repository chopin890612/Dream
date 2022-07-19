using UnityEngine;

namespace Bang.StateMachine.EnemyMachine
{
    public class AbilityState : State<Enemy, EnemyData>
    {
        public AbilityState(Enemy obj, StateMachine<Enemy, EnemyData> stateMachine, EnemyData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}