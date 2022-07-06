using UnityEngine;

namespace Bang.StateMachine
{
    /// <summary>
    /// Base State
    /// </summary>
    /// <typeparam name="T">The class have states.</typeparam>
    /// <typeparam name="T2">The class store needed datas.</typeparam>
    public abstract class State<T, T2>
    {
        protected T obj;
        protected StateMachine<T, T2> stateMachine;
        protected T2 objData;

        protected float startTime;

        public bool isExitingState { get; private set; }

        public State(T obj, StateMachine<T, T2> stateMachine, T2 objData)
        {
            this.obj = obj;
            this.stateMachine = stateMachine;
            this.objData = objData;
        }

        public virtual void EnterState()
        {
            startTime = Time.time;
            isExitingState = false;
        }
        public virtual void ExitState()
        {
            isExitingState = true;
        }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
    }
}