using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.EnemyMachine;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    #region State Varibles
    public StateMachine<Enemy, EnemyData> stateMachine;
    public IdleState idleState;

    [SerializeField] private EnemyData enemyData;
    #endregion

    #region Component
    #endregion

    #region Animations
    #endregion

    #region CHECKS PARAMETERS
    #endregion

    #region Unity LifeCycles
    private void Awake()
    {
        stateMachine = new StateMachine<Enemy, EnemyData>();
        idleState = new IdleState(this, stateMachine, enemyData);
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Movment Methods
    #endregion

    #region CombatMethods
    #endregion
}
