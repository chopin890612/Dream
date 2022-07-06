namespace Bang.StateMachine
{
    /// <summary>
    /// Base StateMachine
    /// </summary>
    /// <typeparam name="T">The class have states.</typeparam>
    /// <typeparam name="T2">The class store needed datas.</typeparam>
    public class StateMachine<T, T2>
    {
        public State<T, T2> currentState { get; private set; }

        public void Initalize(State<T,T2> startState)
        {
            currentState = startState;
            currentState.EnterState();
        }
        public void ChangeState(State<T,T2> tartgetState)
        {
            if (currentState.isExitingState)
                return;

            currentState.ExitState();
            currentState = tartgetState;
            currentState.EnterState();
            //Debug.Log(currentState.ToString());
        }
    }
}