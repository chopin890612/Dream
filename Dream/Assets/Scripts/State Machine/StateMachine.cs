using System;
using System.Collections.Generic;

namespace Bang.State
{
    public class StateMachine
    {
        private List<State> states = new List<State>();
        private State initalState;
        private State currenState;

        //Logic
        private StateTransition triggeredTransition;
        private Queue<Action> actions = new Queue<Action>();


        public StateMachine(State initState)
        {
            initalState = initState;

            currenState = initState;
            AddAction(initState.EnterState);
        }

        public void AddState(State state)
        {
            states.Add(state);
        }
        public void AddStates(params State[] stateArray)
        {
            for (int i = 0; i < stateArray.Length; i++) states.Add(stateArray[i]);
        }

        public Queue<Action> Tick()
        {
            triggeredTransition = null;

            foreach(StateTransition transition in currenState.transitions)
            {
                if (transition.IsTriggered())
                {
                    triggeredTransition = transition;
                    break;
                }
            }

            if(triggeredTransition != null)
            {
                State targetState = triggeredTransition.targetState;

                AddAction(currenState.ExitState);
                AddAction(triggeredTransition.aciton);
                AddAction(targetState.EnterState);

                currenState = targetState;
            }
            else
            {
                AddAction(currenState.UpdateState);
            }

            return actions;
        }

        private void AddAction(Action a)
        {
            this.actions.Enqueue(a);
        }

        public void ExcuteActions(Queue<Action> actions)
        {
            if (actions == null) return;

            Action a;
            while(actions.Count > 0)
            {
                a = actions.Dequeue();
                a();
            }
        }
    }
}