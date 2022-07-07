using System.Collections;
using UnityEngine;
using Bang.StateMachine;
using Bang.StateMachine.PlayerMachine;



public class TestPlayer : MonoBehaviour
{
    #region State Variables

    public StateMachine<TestPlayer, PlayerData> stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public OnAirState onAirState { get; private set; }
    public DashState dashState { get; private set; }
    public WallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public AttackState attackState { get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Rigidbody _rb { get; private set; }
    public float gravityScale;
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public float LastOnGroundTime;
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    #endregion

    #region Debugger
    [SerializeField] string CurrentState;
    private float groundDistance;
    private float wallDistance;
    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        stateMachine = new StateMachine<TestPlayer, PlayerData>();

        idleState = new IdleState(this, stateMachine,playerData);
        runState = new RunState(this, stateMachine, playerData);
        jumpState = new JumpState(this, stateMachine, playerData);
        onAirState = new OnAirState(this, stateMachine, playerData);
        dashState = new DashState(this, stateMachine, playerData);
        wallSlideState = new WallSlideState(this, stateMachine, playerData);
        wallJumpState = new WallJumpState(this, stateMachine, playerData);
        attackState = new AttackState(this, stateMachine, playerData);

        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        InputHandler.instance.OnJumpPressed += args => OnJump(args);
        InputHandler.instance.OnJumpReleased += args => OnJumpUp(args);
        InputHandler.instance.OnDash += args => OnDash(args);
        InputHandler.instance.OnAttack += args => OnAttack(args);

        SetGravityScale(playerData.gravityScale);

        stateMachine.Initalize(idleState);
        IsFacingRight = true;
    }
    private void Update()
    {
        stateMachine.currentState.LogicUpdate();

        #region CHECKS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        //Ground Check
        if (Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSize, _groundLayer)) //checks if set box overlaps with ground
            LastOnGroundTime = playerData.coyoteTime; //if so sets the lastGrounded to coyoteTime

        //Right Wall Check
        if ((Physics.CheckBox(_frontWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _wallLayer) && IsFacingRight)
                || (Physics.CheckBox(_backWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _wallLayer) && !IsFacingRight))
            LastOnWallRightTime = playerData.coyoteTime;

        //Right Wall Check
        if ((Physics.CheckBox(_frontWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _wallLayer) && !IsFacingRight)
            || (Physics.CheckBox(_backWallCheckPoint.position, _wallCheckSize, Quaternion.identity, _wallLayer) && IsFacingRight))
            LastOnWallLeftTime = playerData.coyoteTime;

        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
        #endregion

        CurrentState = stateMachine.currentState.ToString();
    }
    private void FixedUpdate()
    {
        UseGravity();
        stateMachine.currentState.PhysicsUpdate();        
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize * 2);
    }

    

    private void Turn()
    {
        //Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
        //scale.x *= -1;
        //transform.localScale = scale;

        transform.Rotate(0, 180, 0);

        IsFacingRight = !IsFacingRight;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }
    private void UseGravity()
    {
        Physics.gravity = playerData.gravity * gravityScale;
    }
    public void SetGravityScale(float scaleValue)
    {
        gravityScale = scaleValue;
    }

    #region INPUT CALLBACKS
    //These functions are called when an even is triggered in my InputHandler. You could call these methods through a if(Input.GetKeyDown) in Update
    public void OnJump(InputHandler.InputArgs args)
    {
        LastPressedJumpTime = playerData.jumpBufferTime;
    }

    public void OnJumpUp(InputHandler.InputArgs args)
    {
        if (jumpState.CanJumpCut())
            JumpCut();
    }

    public void OnDash(InputHandler.InputArgs args)
    {
        LastPressedDashTime = playerData.dashBufferTime;
    }
    public void OnAttack(InputHandler.InputArgs args)
    {

    }
    #endregion

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
    public void Run(float lerpAmount)
    {
        float targetSpeed = InputHandler.instance.Movement.x * playerData.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
        float speedDif = targetSpeed - _rb.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;

        //gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel : playerData.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.runAccel * playerData.accelInAir : playerData.runDeccel * playerData.deccelInAir;

        //if we want to run but are already going faster than max run speed
        if (((_rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (_rb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && playerData.doKeepRunMomentum)
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = playerData.stopPower;
        }
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = playerData.turnPower;
        }
        else
        {
            velPower = playerData.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount);

        _rb.AddForce(movement * Vector2.right); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

        if (InputHandler.instance.Movement.x != 0)
            CheckDirectionToFace(InputHandler.instance.Movement.x > 0);
    }
    public void Jump()
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        float adjustedJumpForce = playerData.jumpForce;
        if (_rb.velocity.y < 0)
            adjustedJumpForce -= _rb.velocity.y;

        _rb.AddForce(Vector2.up * adjustedJumpForce, ForceMode.Impulse);
        #endregion
    }
    private void JumpCut()
    {
        //applies force downward when the jump button is released. Allowing the player to control jump height
        _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - playerData.jumpCutMultiplier), ForceMode.Impulse);
    }
    public void Dash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        _rb.velocity = dir.normalized * playerData.dashSpeed;

        SetGravityScale(0);
    }
    public void Slide()
    {
        //works the same as the Run but only in the y-axis
        float targetSpeed = 0;
        float speedDif = targetSpeed - _rb.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * playerData.slideAccel, playerData.slidePower) * Mathf.Sign(speedDif);
        _rb.AddForce(movement * Vector2.up, ForceMode.Force);
    }
    public void WallJump(int dir)
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump
        Vector2 force = new Vector2(playerData.wallJumpForce.x, playerData.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(_rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= _rb.velocity.x;

        if (_rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= _rb.velocity.y;

        _rb.AddForce(force, ForceMode.Impulse);
        #endregion
    }
    #endregion

    #region Combat Methods
    public void Attack()
    {

    }
    #endregion
}
