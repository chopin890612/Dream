using System.Collections.Generic;
using System;

namespace Bang.State
{
    public class State
    {
        public List<StateTransition> transitions = new List<StateTransition>();

        public Action EnterState;
        public Action UpdateState;
        public Action ExitState;

        public State(Action enterState, Action updateState, Action exitState)
        {
            this.EnterState = enterState; 
            this.UpdateState = updateState;
            this.ExitState = exitState;
        }

        public void When(Func<bool> condi, State TState, Action action)
        {
            StateTransition stateTransition = new StateTransition(condi, TState, action);
            transitions.Add(stateTransition);
        }
    }
}