using System;

namespace Bang.State 
{ 
    public class StateTransition
    {
        public Func<bool> condition;
        public State targetState;
        public Action aciton;

        public StateTransition(Func<bool> condi, State TState, Action aciton)
        {
            this.condition = condi;
            this.targetState = TState;
            this.aciton = aciton;
        }

        public bool IsTriggered()
        {
            if (condition == null)
                return true;

            return condition();
        }
    }
}