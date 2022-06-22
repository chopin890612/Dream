using System.Collections;
using System.Collections.Generic;

namespace Bang.StateMachine
{
    /// <summary>
    /// Base State
    /// </summary>
    /// <typeparam name="T">The class have states.</typeparam>
    /// <typeparam name="T2">The class store condition params.</typeparam>
    public abstract class State<T, T2>
    {
        protected T obj;
        protected StateMachine<T, T2> stateMachine;
        protected T2 objData;

        public State(T obj, StateMachine<T, T2> stateMachine, T2 objData)
        {
            this.obj = obj;
            this.stateMachine = stateMachine;
            this.objData = objData;
        }

        public virtual void EnterState()
        {
            DoCheck();
        }
        public virtual void ExitState()
        {

        }
        public virtual void LogicUpdate()
        {

        }
        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }
        public void DoCheck()
        {

        }
    }
}