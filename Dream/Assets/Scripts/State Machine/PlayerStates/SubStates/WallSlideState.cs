using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;


namespace Bang.StateMachine.PlayerMachine
{
    public class WallSlideState : OnWallState
    {
        public WallSlideState(TestPlayer obj, StateMachine<TestPlayer, PlayerData> stateMachine, PlayerData objData) : base(obj, stateMachine, objData)
        {
        }
    }
}