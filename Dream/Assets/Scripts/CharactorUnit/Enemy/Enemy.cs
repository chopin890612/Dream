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
    public RunState runState;

    [SerializeField] private EnemyData enemyData;
    #endregion

    #region State Parameters
    public bool CanSlope;
    #endregion

    #region Component
    private Rigidbody _rb;
    #endregion

    #region Animations
    #endregion

    #region CHECKS PARAMETERS
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckSize;
    [SerializeField] private bool _onGround;

    [SerializeField] private Transform _wallCheckPoint;
    [SerializeField] private Vector3 _wallCheckSize;
    [SerializeField] private bool _onWall;

    [SerializeField] private float _platformSlopeCheckLength;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    #endregion

    [SerializeField] private int faceDir = 1;
    private Vector2 _platformNormal;
    [SerializeField] private string currentState;

    #region Unity LifeCycles
    private void Awake()
    {
        stateMachine = new StateMachine<Enemy, EnemyData>();
        idleState = new IdleState(this, stateMachine, enemyData);
        runState = new RunState(this, stateMachine, enemyData);
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        stateMachine.Initalize(runState);
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();

        #region CHECKS
        //Ground Check
        _onGround = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSize, _groundLayer);

        //Wall Check
        _onWall = Physics.CheckBox(_wallCheckPoint.position, _wallCheckSize, Quaternion.identity, _wallLayer);

        //Platform Slope Check
        Physics.Raycast(transform.position, Vector2.down, out RaycastHit hit, _platformSlopeCheckLength);
        _platformNormal = hit.normal;
        #endregion

        if(_onGround == false || _onWall == true || CanSlope == false)
        {
            Turn();
        }

        CanSlope = Mathf.Abs(Vector2.Angle(_platformNormal, Vector2.up)) < enemyData.maxSlopeAngle;

        currentState = stateMachine.currentState.ToString();
    }
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_wallCheckPoint.position, _wallCheckSize * 2);
    }

    private void Turn()
    {
        //Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
        //scale.x *= -1;
        //transform.localScale = scale;
        transform.Rotate(0, 180, 0);

        faceDir *= -1;
    }

    #region Movment Methods
    public void Drag(float amount)
    {
        Vector2 force = amount * _rb.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(force.x)); //ensures we only slow the player down, if the player is going really slowly we just apply a force stopping them
        force.y = Mathf.Min(Mathf.Abs(_rb.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(_rb.velocity.x); //finds direction to apply force
        force.y *= Mathf.Sign(_rb.velocity.y);

        _rb.AddForce(-force, ForceMode.Impulse); //applies force against movement direction
    }
    public void Run(float lerpAmount, bool walkSlope)
    {
        float targetSpeed = enemyData.runMaxSpeed * faceDir;
        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? enemyData.runAccel : enemyData.runDeccel;
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = enemyData.stopPower;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = enemyData.turnPower;
        }
        else
        {
            velPower = enemyData.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount);

        Vector2 moveDir = Vector2.right;
        if (walkSlope && CanSlope)
        {
            moveDir = Vector2.Perpendicular(_platformNormal).normalized * -1;
            Debug.DrawRay(transform.position, moveDir, Color.red);
        }
        _rb.AddForce(movement * moveDir); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 
    }
    #endregion

    #region CombatMethods
    #endregion
}
